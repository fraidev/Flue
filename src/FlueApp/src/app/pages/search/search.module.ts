import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';

import { SearchPageRoutingModule } from './search-routing.module';
import { SearchPage } from './search';
import { SearchFilterPage } from './search-filter/Search-filter';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    SearchPageRoutingModule
  ],
  declarations: [
    SearchPage,
    SearchFilterPage
  ],
  entryComponents: [
    SearchFilterPage
  ]
})
export class SearchModule { }
