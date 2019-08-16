import { Injectable } from '@angular/core';
import { Events } from '@ionic/angular';
import { Storage } from '@ionic/storage';
import { User } from '../shared/models';
import { AuthenticationService, UserService } from './services';
import { first } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class UserDataService {
  _favorites: string[] = [];
  HAS_LOGGED_IN = 'hasLoggedIn';
  HAS_SEEN_TUTORIAL = 'hasSeenTutorial';

  constructor(
    public events: Events,
    public storage: Storage,
    public authenticationService: AuthenticationService,
    public userService: UserService
  ) { }

  hasFavorite(sessionName: string): boolean {
    return (this._favorites.indexOf(sessionName) > -1);
  }

  addFavorite(sessionName: string): void {
    this._favorites.push(sessionName);
  }

  removeFavorite(sessionName: string): void {
    const index = this._favorites.indexOf(sessionName);
    if (index > -1) {
      this._favorites.splice(index, 1);
    }
  }

  public login(user: User): Promise<any> {
    // return this.storage.set(this.HAS_LOGGED_IN, true).then(() => {
    //   this.setUsername(user.username);
    //   return this.events.publish('user:login', user);
    // });
    return this.authenticationService.login(user.username, user.password).toPromise();
  }

  public signup(user: User): Promise<any> {
    // return this.storage.set(this.HAS_LOGGED_IN, true).then(() => {
    //   this.setUsername(user.username);
    //   return this.events.publish('user:signup', user);
    // });
    return this.userService.register(user).toPromise();
  }

  logout(): Promise<any> {
    return this.storage.remove(this.HAS_LOGGED_IN).then(() => {
      return this.storage.remove('username');
    }).then(() => {
      this.events.publish('user:logout');
    });
  }

  setUsername(username: string): Promise<any> {
    return this.storage.set('username', username);
  }

  getUsername(): Promise<string> {
    return this.storage.get('username').then((value) => {
      return value;
    });
  }

  isLoggedIn(): Promise<boolean> {
    return this.storage.get(this.HAS_LOGGED_IN).then((value) => {
      return value === true;
    });
  }

  checkHasSeenTutorial(): Promise<string> {
    return this.storage.get(this.HAS_SEEN_TUTORIAL).then((value) => {
      return value;
    });
  }
}
