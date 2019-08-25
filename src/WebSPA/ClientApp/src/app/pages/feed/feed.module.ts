import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';

import { FeedPageRoutingModule } from './feed-routing.module';
import { FeedPage } from './feed';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    FeedPageRoutingModule,
    SharedModule
  ],
  declarations: [
    FeedPage
  ],
  entryComponents: [
  ]
})
export class FeedModule { }
