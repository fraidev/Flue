import { Component, OnInit } from '@angular/core';
import { PersonService } from 'src/app/shared/services';
import { ActivatedRoute } from '@angular/router';
import { Person } from 'src/app/shared/models';

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.scss']
})
export class PeopleComponent implements OnInit {
  public people: Person[];
  searchText = '';

  constructor(private personService: PersonService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(val => {
      this.searchText = val.searchText;
      this.personService.getPeople(val.searchText).subscribe(x => {
        this.people = x;
        console.log(this.people);
      });
    });
  }
}
