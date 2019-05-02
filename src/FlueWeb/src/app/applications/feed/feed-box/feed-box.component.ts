import { Component, OnInit, Input } from '@angular/core';
import { FeedService } from '../feed.service';
import { AuthenticationService } from 'src/app/shared/services';

@Component({
  selector: 'app-feed-box',
  templateUrl: './feed-box.component.html',
  styleUrls: ['./feed-box.component.scss']
})
export class FeedBoxComponent implements OnInit {
  public text: any;

  constructor(private feedApi: FeedService, private authenticationService: AuthenticationService) {
  }

  ngOnInit() {
    console.log(this.authenticationService.currentUserValue.token);
    // this.feedApi.indentity().subscribe(x => console.log(x));
  }

  public post() {
    const post = {
      text: this.text
    };
    this.feedApi.createPost(post).subscribe();
  }
}
