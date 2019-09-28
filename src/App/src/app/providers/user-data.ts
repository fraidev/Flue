import { Injectable } from '@angular/core';
import { Events } from '@ionic/angular';
import { Storage } from '@ionic/storage';
import { User, Person } from '../shared/models';
import { AuthenticationService, UserService, PeopleService } from '../services';
import { first } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class UserDataService {
  HAS_LOGGED_IN = 'hasLoggedIn';
  HAS_SEEN_TUTORIAL = 'hasSeenTutorial';

  constructor(
    public events: Events,
    public storage: Storage,
    public authenticationService: AuthenticationService,
    public userService: UserService,
    public peopleService: PeopleService
  ) { }

  public async login(user: User): Promise<any> {
    await this.authenticationService.login(user.username, user.password).toPromise();
    return this.events.publish('user:login', user);
  }

  public async signup(user: User): Promise<any> {
    await this.userService.register(user).toPromise();
    return this.events.publish('user:signup', user);
  }

  async logout(): Promise<any> {
    await this.authenticationService.logout();
    return this.events.publish('user:logout');
  }

  setUsername(username: string): Promise<any> {
    return this.storage.set('username', username);
  }

  async getUser(): Promise<User> {
    const value = await this.storage.get('currentUser');
    return value;
  }

  isLoggedIn(): Promise<boolean> {
    return this.authenticationService.isLoggedIn;
  }

  async checkHasSeenTutorial(): Promise<string> {
    const value = await this.storage.get(this.HAS_SEEN_TUTORIAL);
    return value;
  }
}
