import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileBoxComponent } from './profile-box/profile-box.component';
import { SharedModule } from '../shared.module';
import { MaterialModule } from '../material.module';

@NgModule({
  declarations: [ProfileBoxComponent],
  exports: [
    ProfileBoxComponent
  ],
  imports: [
    CommonModule,
    MaterialModule
  ]
})
export class LayoutModule { }
