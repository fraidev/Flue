import { Component } from '@angular/core';
import { FeedService } from '../../services/feed.service';

@Component({
  selector: 'page-schedule',
  templateUrl: 'feed.html',
  styleUrls: ['./feed.scss'],
})
export class FeedPage {
  public posts: any[] = [];
  public comment: any;

  constructor(
    public feedApi: FeedService,
  ) { }

  ionViewDidEnter() {
    this.feedApi.getMyFeed().subscribe(x => this.posts = x);
  }
}
