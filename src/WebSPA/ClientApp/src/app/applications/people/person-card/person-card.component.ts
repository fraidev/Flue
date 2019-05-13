import { Component, OnInit, Input } from '@angular/core';
import { PeopleService } from 'src/app/shared/services';
import { Person } from 'src/app/shared/models';

@Component({
  selector: 'app-person-card',
  templateUrl: './person-card.component.html',
  styleUrls: ['./person-card.component.scss']
})
export class PersonCardComponent implements OnInit {
  @Input() private content: Person;
  public img: string;
  public following: boolean;

  constructor(private peopleService: PeopleService) { }

  ngOnInit() {
    console.log(this.content);
    this.getAvatar();
    this.following = this.peopleService.cotainsFollowing(this.content.personId);
  }

  private getAvatar() {
    this.img = this.content.profilePicture ? this.content.profilePicture : `/assets/img/profile.png`;
  }

  private follow() {
    this.peopleService.follow(this.content.personId);
    this.following = true;
  }

  private unfollow() {
    this.peopleService.unfollow(this.content.personId);
    this.following = false;
  }
}
