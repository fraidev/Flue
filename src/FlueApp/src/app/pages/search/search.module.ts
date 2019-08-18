import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IonicModule } from '@ionic/angular';

import { MapPageRoutingModule } from './search-routing.module';
import { SearchPage } from './search';

@NgModule({
  imports: [
    CommonModule,
    IonicModule,
    MapPageRoutingModule
  ],
  declarations: [
    SearchPage,
  ]
})
export class MapModule { }
