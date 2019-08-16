import { Component, ViewEncapsulation } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { UserDataService } from '../../providers/user-data';

import { UserOptions } from '../../interfaces/user-options';
import { User } from '../../shared/models';



@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
  styleUrls: ['./login.scss'],
})
export class LoginPage {
  login = new User();
  submitted = false;

  constructor(
    public userData: UserDataService,
    public router: Router
  ) { }

  onLogin(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.userData.login(this.login);
    }
  }

  onSignup() {
    this.router.navigateByUrl('/signup');
  }
}
