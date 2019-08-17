import { Component, ViewEncapsulation } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { UserDataService } from '../../providers/user-data';

import { UserOptions } from '../../interfaces/user-options';
import { User } from '../../shared/models';
import { async } from '@angular/core/testing';
import { ToastController } from '@ionic/angular';



@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
  styleUrls: ['./login.scss'],
})
export class LoginPage {
  login = new User();
  submitted = false;

  constructor(
    public userData: UserDataService,
    public router: Router,
    public toastCtrl: ToastController) { }

  onLogin(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.userData.login(this.login).then(async () => {
        this.submitted = false;

        const toast = await this.toastCtrl.create({
          message: `Conta ${this.login.username} criada`,
          duration: 3000
        });
        await toast.present();
      });
    }
  }

  onSignup() {
    this.router.navigateByUrl('/signup');
  }
}
