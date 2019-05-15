import { Component, OnInit } from '@angular/core';
import { FeedService } from './feed.service';
import { Post } from 'src/app/shared/models';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {
  public title = 'Feed';
  public cards: Post[];
  constructor(private feedService: FeedService) {
  }

  ngOnInit() {
    this.feedService.getMyFeed().subscribe(x => {
      this.cards = x;
      console.log(this.cards);
    });
  }
}
