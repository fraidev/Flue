import { Component, ViewEncapsulation } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { UserDataService } from '../../providers/user-data';

import { UserOptions } from '../../interfaces/user-options';
import { User } from '../../shared/models';
import { ToastController } from '@ionic/angular';



@Component({
  selector: 'page-signup',
  templateUrl: 'signup.html',
  styleUrls: ['./signup.scss'],
})
export class SignupPage {
  signup = new User();
  submitted = false;

  constructor(
    public router: Router,
    public userData: UserDataService,
    public toastCtrl: ToastController
  ) { }

  onSignup(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.userData.signup(this.signup).then(async () => {
        this.submitted = false;

        const toast = await this.toastCtrl.create({
          message: `Accesso a conta do usuario ${this.signup.username} permitida`,
          duration: 3000
        });
        await toast.present();
      });
      this.router.navigateByUrl('/app/tabs/feed');
    }
  }
}
