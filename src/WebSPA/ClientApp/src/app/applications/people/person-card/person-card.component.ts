import { Component, OnInit, Input } from '@angular/core';
import { PersonService } from 'src/app/shared/services';

@Component({
  selector: 'app-person-card',
  templateUrl: './person-card.component.html',
  styleUrls: ['./person-card.component.scss']
})
export class PersonCardComponent implements OnInit {
  @Input() private content: any;
  public img: string;
  public following: boolean;

  constructor(private personService: PersonService) { }

  ngOnInit() {
    console.log(this.content);
    this.getAvatar();
    this.following = this.personService.cotainsFollowing(this.content.personId);
  }

  private getAvatar() {
    this.img = this.content.profilePicture ? this.content.profilePicture : `/assets/img/profile.png`;
  }

  private follow() {
    this.personService.follow(this.content.personId);
    this.following = true;
  }

  private unfollow() {
    this.personService.unfollow(this.content.personId);
    this.following = false;
  }
}
