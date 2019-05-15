import { Component, OnInit, Input } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Post } from 'src/app/shared/models';

@Component({
  selector: 'app-feed-card',
  templateUrl: './feed-card.component.html',
  styleUrls: ['./feed-card.component.scss']
})
export class FeedCardComponent implements OnInit {
  @Input() private content: Post;
  public img: string;

  constructor() { }

  ngOnInit() {
    this.getAvatar();
  }


  private getAvatar() {
      this.img = this.content.person.profilePicture ? this.content.person.profilePicture : `/assets/img/profile.png`;
  }
}
