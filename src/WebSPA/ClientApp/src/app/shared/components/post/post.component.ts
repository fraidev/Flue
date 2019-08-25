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
    @Input()
    public person: Person;

    public commentText: string;

    get getPerson() {
        return this.person ? this.person : this.post.person;
    }

    get getAvatar() {
        return this.getPerson.profilePicture ? this.getPerson.profilePicture : `/assets/img/profile.png`;
    }

    constructor(private feedService: FeedService) { }

    ngOnInit(): void { }

    sendComment() {
        const cmd = {
            id: uuid(),
            postId: this.post.postId,
            text: this.commentText
        };
        this.feedService.addComment(cmd).subscribe();
    }




}
