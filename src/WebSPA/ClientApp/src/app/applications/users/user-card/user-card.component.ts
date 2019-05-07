import { Component, OnInit, Input } from '@angular/core';
import { UserService } from 'src/app/shared/services';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.scss']
})
export class UserCardComponent implements OnInit {
  @Input() private content: any;
  public img: string;
  public following: boolean;

  constructor(private userService: UserService) { }

  ngOnInit() {
    console.log(this.content);
    this.getAvatar();
    this.following = this.userService.cotainsFollowing(this.content.id);
  }

  private getAvatar() {
    this.img = this.content.profilePicture ? this.content.profilePicture : `/assets/img/profile.png`;
  }

  private follow() {
    this.userService.follow(this.content.id);
    this.following = true;
  }

  private unfollow() {
    this.userService.unfollow(this.content.id);
    this.following = false;
  }
}
