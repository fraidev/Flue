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
export class AccountPage {
  username: string;
  user: User;
  person: Person;
  posts: Post[];
  postsCount: number;
  followingCount: any;
  followersCount: any;
  anotherAccount: boolean;

  constructor(
    public alertCtrl: AlertController,
    public router: Router,
    public userData: UserDataService,
    public feedApi: FeedService,
    public peopleApi: PeopleService,
    public popoverCtrl: PopoverController) {
    const currentNavigation = this.router.getCurrentNavigation();
    if (currentNavigation
      && currentNavigation.extras
      && currentNavigation.extras.state
      && currentNavigation.extras.state.person) {
      this.person = currentNavigation.extras.state.person;
      this.anotherAccount = true;
    }
  }

  onShowFollowings() {
    this.router.navigateByUrl('search',
      { state: { searchPeopleType: 'Followings', personId: this.person.personId } });
  }

  onShowFollowers() {
    this.router.navigateByUrl('search',
      { state: { searchPeopleType: 'Followers', personId: this.person.personId } });
  }

  async presentPopover(event: Event) {
    const popover = await this.popoverCtrl.create({
      component: AccountPopover,
      event
    });
    await popover.present();
  }

  get getAvatar() {
    return this.person.profilePicture ? this.person.profilePicture : `/assets/img/profile.png`;
  }

  ionViewDidEnter() {
    if (!this.anotherAccount) {
      this.peopleApi.getMe().subscribe(x => this.person = x);
    }
    this.getPosts();
  }

  getPosts() {
    if (this.person && this.person.personId) {
      this.feedApi.getPostsByPersonId(this.person.personId).subscribe((posts) => {
        this.posts = posts;
      });
    }
  }

}
