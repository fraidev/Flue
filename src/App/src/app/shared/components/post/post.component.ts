import { Component, OnInit, Input } from '@angular/core';
import { Post, Person, Comment } from '../../models';
import { FeedService } from '../../../services/feed.service';
import { v4 as uuid } from 'uuid';
import { AlertController, Events } from '@ionic/angular';

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
    public deleted: boolean;

    constructor(private feedService: FeedService,
        private alertController: AlertController,
        public events: Events) { }

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

    async deletePost() {
        const alert = await this.alertController.create({
            header: 'Deletar Post!',
            message: 'Tem certeza sobre a <strong>Remoção</strong> do seu Post?',
            buttons: [
                {
                    text: 'Cancel',
                    role: 'cancel',
                    cssClass: 'secondary',
                    handler: (blah) => {
                    }
                }, {
                    text: 'Okay',
                    handler: () => {
                        this.feedService.deletePost(this.post.postId).subscribe(() =>
                            this.deleted = true
                        );
                    }
                }
            ]
        });

        await alert.present();
    }

    async deleteComment(comment: Comment) {
        const alert = await this.alertController.create({
            header: 'Deletar Comentario!',
            message: 'Tem certeza sobre a <strong>Remoção</strong> do seu Comentario?',
            buttons: [
                {
                    text: 'Cancel',
                    role: 'cancel',
                    cssClass: 'secondary',
                    handler: (blah) => {
                    }
                }, {
                    text: 'Okay',
                    handler: () => {
                        this.feedService.removeComment(this.post.postId, comment.commentId).subscribe(() =>
                            this.feedService.getPostById(this.post.postId).subscribe(x => this.post = x)
                        );
                    }
                }
            ]
        });

        await alert.present();
    }
}
