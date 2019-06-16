import { Component, OnInit, Input } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from 'src/app/shared/models';
import { FeedService } from '../feed.service';
import { FeedComponent } from '../feed.component';

@Component({
  selector: 'app-feed-card',
  templateUrl: './feed-card.component.html',
  styleUrls: ['./feed-card.component.scss']
})
export class FeedCardComponent implements OnInit {
  @Input() private content: Post;
  @Input() private feedComponent: FeedComponent;
  public img: string;

  constructor(public feedService: FeedService) { }

  ngOnInit() {
    this.getAvatar();
  }

  public deletePost(id: string) {
    this.feedService.deletePost(id).subscribe(x => {
      this.feedComponent.cards.splice(this.feedComponent.cards.findIndex(card => card.postId === this.content.postId), 1);
    });
  }


  private getAvatar() {
      this.img = this.content.person.profilePicture ? this.content.person.profilePicture : `/assets/img/profile.png`;
  }
}
