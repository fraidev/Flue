import { Component, OnInit, Input } from '@angular/core';
import { Person } from '../../models';
import { PeopleService } from '../../services';
import { ProfileBoxService } from './profile-box.service';

@Component({
  selector: 'app-profile-box',
  templateUrl: './profile-box.component.html',
  styleUrls: ['./profile-box.component.scss']
})
export class ProfileBoxComponent implements OnInit {
  private person: Person;
  public img = '';
  private followingCount: number;
  private followersCount: number;
  private postsCount: number;


  constructor(private peopleService: PeopleService, private profileBoxService: ProfileBoxService) {}

  ngOnInit() {
    this.followingCount = this.peopleService.following.length;
    this.followersCount = this.peopleService.followers.length;
    this.person = this.peopleService.me;
    this.profileBoxService.GetMyPostCount().subscribe(x => this.postsCount = x);
    this.getAvatar();
  }

  private getAvatar() {
      const profilePicture = this.person ? this.person.profilePicture : null;
      this.img = profilePicture ? this.person.profilePicture : `/assets/img/profile.png`;
  }
}
