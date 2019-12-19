import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { TabsPage } from './tabs-page';
import { TabsPageRoutingModule } from './tabs-page-routing.module';

import { FeedModule } from '../feed/feed.module';
import { SearchModule } from '../search/search.module';
import { SupportModule } from '../support/support.module';

@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    SearchModule,
    FeedModule,
    SupportModule,
    TabsPageRoutingModule
  ],
  declarations: [
    TabsPage,
  ]
})
export class TabsModule { }
