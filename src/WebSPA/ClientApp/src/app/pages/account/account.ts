import { AfterViewInit, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController, PopoverController } from '@ionic/angular';
import { UserDataService } from '../../providers/user-data';
import { User, Person, Post } from '../../shared/models';
import { PeopleService } from '../../services';
import { FeedService } from '../../services/feed.service';
import { AccountPopover } from './account-popover/account-popover';


@Component({
  selector: 'page-account',
  templateUrl: 'account.html',
  styleUrls: ['./account.scss'],
})
export class AccountPage implements AfterViewInit {
  username: string;
  user: User;
  person: Person;
  postsCount: number;
  followingCount: any;
  followersCount: any;
  posts: Post[];

  constructor(
    public alertCtrl: AlertController,
    public router: Router,
    public userData: UserDataService,
    public feedApi: FeedService,
    public peopleApi: PeopleService,
    public popoverCtrl: PopoverController) { }

  async presentPopover(event: Event) {
    const popover = await this.popoverCtrl.create({
      component: AccountPopover,
      event
    });
    await popover.present();
  }
  ngAfterViewInit() {
    this.getPerson();
    this.getPosts();
  }

  updatePicture() {
    console.log('Clicked to update picture');
  }

  getPerson() {
    this.peopleApi.getMe().subscribe((person) => {
      this.person = person;
    });
  }

  getPosts() {
    this.feedApi.getMyFeed().subscribe((posts) => {
      this.posts = posts;
    });
  }

}
