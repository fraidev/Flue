import { Component, OnInit, Input } from '@angular/core';
import { Post, Comment } from 'src/app/shared/models';
import { FeedService } from '../feed.service';
import { FeedComponent } from '../feed.component';
import * as uuid from 'uuid';

@Component({
  selector: 'app-feed-card',
  templateUrl: './feed-card.component.html',
  styleUrls: ['./feed-card.component.scss']
})
export class FeedCardComponent implements OnInit {
  @Input() private content: Post;
  @Input() private feedComponent: FeedComponent;
  public visibleComments: Comment[];
  public img: string;
  public text: any;
  public visibleCommentsCount = 2;

  constructor(public feedService: FeedService) { }

  ngOnInit() {
    this.getAvatar();
    this.visibleComments = this.content.comments.slice(0, this.visibleCommentsCount);
  }

  public getMoreVisibleComments() {
    this.visibleCommentsCount += 3;
    this.visibleComments = this.content.comments.slice(0, this.visibleCommentsCount);
  }

  public deletePost(id: string) {
    this.feedService.deletePost(id).subscribe(x => {
      this.feedComponent.cards.splice(this.feedComponent.cards.findIndex(card => card.postId === this.content.postId), 1);
    });
  }

  public removeComment(id: string) {
    this.feedService.removeComment(id).subscribe(
//TODO remove 
    );
  }

  public commentPost() {
    const cmd = {
      id: uuid.v4(),
      text: this.text,
      postId: this.content.postId
    };

    this.feedService.addComment(cmd).subscribe();
  }

  private getAvatar() {
      this.img = this.content.person.profilePicture ? this.content.person.profilePicture : `/assets/img/profile.png`;
  }
}
