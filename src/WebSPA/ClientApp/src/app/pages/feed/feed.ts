import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActionSheetController, ModalController } from '@ionic/angular';
import { InAppBrowser } from '@ionic-native/in-app-browser/ngx';
import { FeedService } from '../../services/feed.service';

@Component({
  selector: 'page-schedule',
  templateUrl: 'feed.html',
  styleUrls: ['./feed.scss'],
})
export class FeedPage {
  public posts: any[] = [];
  public comment: any;

  constructor(
    public actionSheetCtrl: ActionSheetController,
    public inAppBrowser: InAppBrowser,
    public router: Router,
    public feedApi: FeedService,
    public modalCtrl: ModalController
  ) { }

  ionViewDidEnter() {
    this.feedApi.getMyFeed().subscribe(x => this.posts = x);
  }
}
