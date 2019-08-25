import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { AccountPage } from './account';
import { AccountPageRoutingModule } from './account-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { AccountPopover } from './account-popover/account-popover';

@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    AccountPageRoutingModule,
    SharedModule
  ],
  declarations: [
    AccountPage,
    AccountPopover
  ],
  entryComponents: [
    AccountPopover
  ]
})
export class AccountModule { }
