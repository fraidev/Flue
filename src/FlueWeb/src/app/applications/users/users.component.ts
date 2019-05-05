import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/shared/services';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  public users: any[];
  searchText = '';

  constructor(private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(val => {
      this.searchText = val.searchText;
      this.userService.getUsers(val.searchText).subscribe(x => {
        this.users = x;
        console.log(this.users);
      });
    });
  }
}
