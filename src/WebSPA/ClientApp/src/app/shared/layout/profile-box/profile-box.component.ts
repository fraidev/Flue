import { Component, OnInit } from '@angular/core';
import { Person } from '../../models';
import { PeopleService } from '../../services';
import { ProfileBoxService } from './profile-box.service';

@Component({
  selector: 'app-profile-box',
  templateUrl: './profile-box.component.html',
  styleUrls: ['./profile-box.component.scss']
})
export class ProfileBoxComponent implements OnInit {
  public img: string;
  public person: Person;
  private followingCount: number;
  private followersCount: number;
  private postsCount: number;

  constructor(private peopleService: PeopleService, private profileBoxService: ProfileBoxService) { }

  ngOnInit() {
    this.getAvatar();
    this.followingCount = this.peopleService.following.length;
    this.followersCount = this.peopleService.followers.length;
    this.profileBoxService.GetMyPostCount().subscribe(x => this.postsCount = x);
  }


  private getAvatar() {
      this.img = this.person.profilePicture ? this.person.profilePicture : `/assets/img/profile.png`;
  }
}
