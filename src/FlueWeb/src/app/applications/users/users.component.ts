import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/shared/services';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  public users: any[];

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getAll().subscribe(x => this.users = x);
  }

}
