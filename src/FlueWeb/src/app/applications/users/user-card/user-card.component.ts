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

  constructor(private userService: UserService) { }

  ngOnInit() {
    console.log(this.content);
    this.getAvatar();
  }


  private getAvatar() {
      this.img = this.content.profilePicture ? this.content.profilePicture : `/assets/img/profile.png`;
  }

  private follow() {
    this.userService.follow(this.content.id).subscribe();
  }
}
