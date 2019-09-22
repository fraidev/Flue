import { Component } from '@angular/core';

import { PopoverController } from '@ionic/angular';
import { Router } from '@angular/router';

@Component({
  template: `
    <ion-list>
      <ion-item button (click)="goToEditAccount()">
        <ion-label>Editar Conta</ion-label>
      </ion-item>
    </ion-list>
  `
})
export class AccountPopover {
  constructor(public popoverCtrl: PopoverController, private router: Router) {}
 
  close(url: string) {
    window.open(url, '_blank');
    this.popoverCtrl.dismiss();
  }

  goToEditAccount() {
    this.router.navigateByUrl('account/edit');
    this.popoverCtrl.dismiss();
  }
}
