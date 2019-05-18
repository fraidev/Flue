import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService, PeopleService } from './shared/services';
import { User, Person } from './shared/models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public searchText = '';
  public currentUser: User;
  title = 'FlueWeb';

  constructor(private router: Router, private authenticationService: AuthenticationService) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  public search() {
    this.router.navigate(['/people', this.searchText]);
  }

  private logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
