import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { UserDataService } from '../../providers/user-data';
import { ToastController } from '@ionic/angular';
import { UserCommand } from '../../shared/models/commands';

@Component({
  selector: 'page-sign-up',
  templateUrl: 'sign-up.html',
  styleUrls: ['./sign-up.scss'],
})
export class SignUpPage {
  signUp = new UserCommand();
  submitted = false;

  constructor(
    public router: Router,
    public userData: UserDataService,
    public toastCtrl: ToastController
  ) { }

  onSignUp(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.userData.signUp(this.signUp).then(async () => {
        this.submitted = false;

        const toast = await this.toastCtrl.create({
          message: `Acesso a conta do usuário ${this.signUp.username} permitida`,
          duration: 3000
        });
        await toast.present();
      });
      this.router.navigateByUrl('/app/tabs/feed');
    }
  }

  onBack() {
    this.router.navigateByUrl('/');
  }
}
