import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FeedService } from './feed.service';
import { Post, Person } from 'src/app/shared/models';
import { PeopleService } from 'src/app/shared/services';
import { ProfileBoxComponent } from 'src/app/applications/feed/profile-box/profile-box.component';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {
  @ViewChild(ProfileBoxComponent) profileBox: ProfileBoxComponent;
  public people: Person[];
  public title = 'Feed';
  public cards: Post[];
  constructor(private peopleService: PeopleService, private feedService: FeedService) {
  }

  ngOnInit() {
    this.peopleService.configure(this.profileBox);
    this.feedService.getMyFeed().subscribe(x => {
      this.cards = x;
      console.log(this.cards);
    });
  }
}
