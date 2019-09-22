import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActionSheetController, ModalController } from '@ionic/angular';

import { InAppBrowser } from '@ionic-native/in-app-browser/ngx';
import { FeedService } from '../../services/feed.service';
import { Person } from '../../shared/models';
import { PeopleService } from '../../services';

@Component({
  selector: 'page-search',
  templateUrl: 'search.html',
  styleUrls: ['./search.scss'],
})
export class SearchPage implements OnInit {
  public queryText: string;
  public people: Person[];

  constructor(
    public router: Router,
    public feedApi: FeedService,
    public modalCtrl: ModalController,
    private peopleService: PeopleService
  ) {
  }

  ionViewDidEnter() {
  }

  ngOnInit(): void {
    this.peopleService.getPeople(this.queryText).subscribe(x => this.people = x);
  }

  updateSearch() {
    this.peopleService.getPeople(this.queryText).subscribe(x => this.people = x);
  }

  public getAvatar(post) {
    return post.profilePicture ? post.profilePicture : `/assets/img/profile.png`;
  }

  public onVisitProfile(person: Person) {
    this.router.navigateByUrl('account', { state: { person } });
  };

  async follow(person: Person) {
    this.peopleService.follow(person.personId)
      .subscribe(() => person.isFollowing = true);
  }

  async unfollow(person: Person) {
    this.peopleService.unfollow(person.personId)
      .subscribe(() => person.isFollowing = false);
  }
}
