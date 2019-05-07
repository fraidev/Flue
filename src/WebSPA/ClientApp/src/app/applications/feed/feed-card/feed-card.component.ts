import { Component, OnInit, Input } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-feed-card',
  templateUrl: './feed-card.component.html',
  styleUrls: ['./feed-card.component.scss']
})
export class FeedCardComponent implements OnInit {
  @Input() private content: any;
  public img: string;

  constructor() { }

  ngOnInit() {
    this.getAvatar();
  }


  private getAvatar() {
      this.img = this.content.profilePicture ? this.content.profilePicture : `/assets/img/profile.png`;
  }
}
