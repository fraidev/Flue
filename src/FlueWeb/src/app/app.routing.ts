import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedComponent } from './applications/feed/feed.component';
import { ChatComponent } from './applications/chat/chat.component';
import { RegisterComponent } from './applications/register/register.component';
import { AuthGuard } from './shared/guards';
import { LoginComponent } from './applications/login/login.component';

const routes: Routes = [
  { path: '', component: FeedComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'chat', component: ChatComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
