import { AfterViewInit, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AlertController } from '@ionic/angular';
import { UserDataService } from '../../providers/user-data';
import { User, Person } from '../../shared/models';
import { PeopleService } from '../../providers/services';
import { FeedService } from '../../providers/services/feed.service';


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

  constructor(
    public alertCtrl: AlertController,
    public router: Router,
    public userData: UserDataService,
    public feedApi: FeedService,
    public peopleApi: PeopleService
  ) { }

  ngAfterViewInit() {
    this.getPerson();
  }

  updatePicture() {
    console.log('Clicked to update picture');
  }

  getPerson() {
    this.peopleApi.getMe().subscribe((person) => {
      this.person = person;
    });
  }

  logout() {
    this.userData.logout();
    this.router.navigateByUrl('/login');
  }

  support() {
    this.router.navigateByUrl('/support');
  }
}
