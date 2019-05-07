import { Component, OnInit } from '@angular/core';
import { FeedService } from './feed.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {
  public title = 'Feed';
  public cards: any[];
  constructor(private feedService: FeedService) {
  }

  ngOnInit() {
    this.feedService.getInbox().subscribe(x => this.cards = x);
  }

}
