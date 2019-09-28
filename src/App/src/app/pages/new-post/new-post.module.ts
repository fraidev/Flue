import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { NewPostPage } from './new-post';
import { NewPostPageRoutingModule } from './new-post-routing.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    NewPostPageRoutingModule
  ],
  declarations: [
    NewPostPage,
  ]
})
export class NewPostModule { }
