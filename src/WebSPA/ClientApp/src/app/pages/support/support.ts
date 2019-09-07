import { Component, ViewEncapsulation } from '@angular/core';
import { NgForm } from '@angular/forms';

import { AlertController, ToastController } from '@ionic/angular';
import { SupportService } from '../../services/support.service';
import { AuthenticationService } from '../../services';


@Component({
  selector: 'page-support',
  templateUrl: 'support.html',
  styleUrls: ['./support.scss'],
})
export class SupportPage {
  submitted = false;
  supportMessage: string;

  constructor(
    public alertCtrl: AlertController,
    public toastCtrl: ToastController,
    private supportService: SupportService,
    private authenticationService: AuthenticationService
  ) { }

  async submit(form: NgForm) {
    this.submitted = true;
    const userId = this.authenticationService.currentUserValue.userId;

    if (form.valid) {
      this.submitted = false;

      this.supportService.createSuportMessage(this.supportMessage, userId)
        .subscribe(async () => {
          const toast = await this.toastCtrl.create({
            message: 'Sua solicitação de suporte foi enviada.',
            duration: 3000
          });
          await toast.present();
        });
    }
  }
}
