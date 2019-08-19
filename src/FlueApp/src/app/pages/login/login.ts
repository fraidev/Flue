import { Component, ViewEncapsulation, OnInit, AfterViewInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { UserDataService } from '../../providers/user-data';

import { UserOptions } from '../../interfaces/user-options';
import { User } from '../../shared/models';
import { async } from '@angular/core/testing';
import { ToastController, Events } from '@ionic/angular';
import { AuthenticationService } from '../../providers/services';



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
    public toastCtrl: ToastController,
    public events: Events) { }

  onLogin(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.userData.login(this.login).then(async () => {
        this.submitted = false;

        const toast = await this.toastCtrl.create({
          message: `Acesso efetuado a conta ${this.login.username}`,
          duration: 1000
        });
        await toast.present();
      });
    }
  }

  async ionViewDidEnter() {
    this.events.publish('login:loginInit');
  }

  onSignup() {
    this.router.navigateByUrl('/signup');
  }
}
