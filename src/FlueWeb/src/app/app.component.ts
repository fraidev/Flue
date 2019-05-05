import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './shared/services';
import { User } from './shared/models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public searchText = '';
  currentUser: User;
  title = 'FlueWeb';

  constructor(private router: Router, private authenticationService: AuthenticationService) {
    this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
  }

  public search() {
    this.router.navigate(['/users', this.searchText]);
  }

  private logout() {
    this.authenticationService.logout();
    this.router.navigate(['/login']);
  }
}
