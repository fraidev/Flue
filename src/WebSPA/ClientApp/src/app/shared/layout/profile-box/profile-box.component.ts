import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile-box',
  templateUrl: './profile-box.component.html',
  styleUrls: ['./profile-box.component.scss']
})
export class ProfileBoxComponent implements OnInit {
  public img: string;

  constructor() { }

  ngOnInit() {
    this.getAvatar();
  }


  private getAvatar() {
      // this.img = this.content.profilePicture ? this.content.profilePicture : `/assets/img/profile.png`;
      this.img = `/assets/img/profile.png`;
  }
}
