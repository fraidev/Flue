import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

import { AlertController, ToastController, IonTextarea } from '@ionic/angular';
import { FeedService } from '../../services/feed.service';
import { Router } from '@angular/router';


@Component({
  selector: 'page-new-post',
  templateUrl: 'new-post.html',
  styleUrls: ['./new-post.scss'],
})
export class NewPostPage {
  submitted = false;
  text: string;

  constructor(
    public router: Router,
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    public feedApi: FeedService
  ) { }

  @ViewChild('postInput', { static: false }) postInput: IonTextarea;

  async submit(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.submitted = false;
      const post = {
        text: this.text
      };
      this.feedApi.createPost(post).subscribe(async () => {
        const toast = await this.toastCtrl.create({
          message: 'Your support request has been sent.',
          duration: 3000
        });
        await toast.present();

        this.router.navigateByUrl('/app/tabs/feed');
      });
    }
  }
}
