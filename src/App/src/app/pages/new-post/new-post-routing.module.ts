import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewPostPage } from './new-post';


const routes: Routes = [
  {
    path: '',
    component: NewPostPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NewPostPageRoutingModule { }
