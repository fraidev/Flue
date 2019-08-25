import { Component, OnInit, Input } from '@angular/core';
import { Post, Person } from '../../models';
import { FeedService } from '../../../services/feed.service';
import { v4 as uuid } from 'uuid';

@Component({
    selector: 'flue-post',
    templateUrl: './post.component.html',
    styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
    @Input()
    public post: Post;
    public commentText: string;
    public moreComments: boolean;

    constructor(private feedService: FeedService) { }

    ngOnInit(): void { }

    get getAvatar() {
        return this.post.person.profilePicture ? this.post.person.profilePicture : `/assets/img/profile.png`;
    }

    get comments() {
        return this.moreComments ? this.post.comments : this.post.comments.slice(0, 3);
    }

    showMoreComments() {
        this.moreComments = true;
    }

    sendComment() {
        const cmd = {
            id: uuid(),
            postId: this.post.postId,
            text: this.commentText
        };
        this.feedService.addComment(cmd).subscribe(() =>
            this.feedService.getPostById(this.post.postId)
                .subscribe(post => this.post = post)
        );
    }
}
