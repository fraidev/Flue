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
  public page = 1;
  public itemsPerPage = 10;

  constructor(
    public feedApi: FeedService,
  ) { }

  ionViewDidEnter() {
    this.refresh();
  }

  loadData(event) {
    if (this.posts.length >= (this.page * this.itemsPerPage)) {
      this.page++;
      this.feedApi.getMyFeed(this.page, this.itemsPerPage)
        .subscribe(x => {
          this.posts.push(...x);
          event.target.complete();
        });
    } else {
      event.target.complete();
    }
  }

  refresh() {
    this.feedApi.getMyFeed(this.page, this.itemsPerPage).subscribe(x => this.posts = x);
  }
}
