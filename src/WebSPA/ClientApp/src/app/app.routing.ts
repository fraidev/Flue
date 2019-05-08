import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedComponent } from './applications/feed/feed.component';
import { ChatComponent } from './applications/chat/chat.component';
import { AuthGuard } from './shared/guards';
import { GamesComponent } from './applications/games/games.component';
import { UsersComponent } from './applications/users/users.component';
import { LoginComponent } from './applications/login/login.component';
import { RegisterComponent } from './applications/register/register.component';

const routes: Routes = [
  { path: '', component: FeedComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'chat', component: ChatComponent, canActivate: [AuthGuard] },
  { path: 'games', component: GamesComponent, canActivate: [AuthGuard] },
  { path: 'users/:searchText', component: UsersComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }