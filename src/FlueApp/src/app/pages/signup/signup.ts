import { Component, ViewEncapsulation } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

import { UserDataService } from '../../providers/user-data';

import { UserOptions } from '../../interfaces/user-options';
import { User } from '../../shared/models';



@Component({
  selector: 'page-signup',
  templateUrl: 'signup.html',
  styleUrls: ['./signup.scss'],
})
export class SignupPage {
  signup = new User();
  submitted = false;

  constructor(
    public router: Router,
    public userData: UserDataService
  ) {}

  onSignup(form: NgForm) {
    this.submitted = true;

    if (form.valid) {
      this.userData.signup(this.signup);
      this.router.navigateByUrl('/app/tabs/schedule');
    }
  }
}
