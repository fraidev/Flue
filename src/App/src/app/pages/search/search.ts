import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ModalController } from '@ionic/angular';
import { FeedService } from '../../services/feed.service';
import { Person } from '../../shared/models';
import { PeopleService } from '../../services';

@Component({
  selector: 'page-search',
  templateUrl: 'search.html',
  styleUrls: ['./search.scss'],
})
export class SearchPage implements OnInit {
  public queryText = '';
  public people: Person[];
  public page = 1;
  public itemsPerPage = 10;
  public searchPeopleType: 'All' | 'Followings' | 'Followers' = 'All';
  public personId: string;


  constructor(
    public router: Router,
    public feedApi: FeedService,
    public modalCtrl: ModalController,
    private peopleService: PeopleService
  ) {
    const currentNavigation = this.router.getCurrentNavigation();
    if (currentNavigation
      && currentNavigation.extras
      && currentNavigation.extras.state
      && currentNavigation.extras.state.searchPeopleType
      && currentNavigation.extras.state.personId) {
      this.searchPeopleType = currentNavigation.extras.state.searchPeopleType;
      this.personId = currentNavigation.extras.state.personId;
    }
  }

  ngOnInit(): void {
    this.updateSearch();
  }

  loadData(event) {
    if (this.people.length >= (this.page * this.itemsPerPage)) {
      this.page++;
      this.peopleService.getPeople(this.queryText, this.page, this.itemsPerPage,
        this.searchPeopleType, this.personId)
        .subscribe(x => {
          this.people.push(...x);
          event.target.complete();
        });
    } else {
      event.target.complete();
    }
  }

  updateSearch() {
    this.peopleService.getPeople(this.queryText, this.page, this.itemsPerPage,
      this.searchPeopleType, this.personId).subscribe(x => this.people = x);
  }

  public getAvatar(post) {
    return post.profilePicture ? post.profilePicture : `/assets/img/profile.png`;
  }

  public onVisitProfile(person: Person) {
    this.router.navigateByUrl('account', { state: { person } });
  }

  async follow(evt, person: Person) {
    evt.stopPropagation();
    this.peopleService.follow(person.personId)
      .subscribe(() => person.isFollowing = true);
  }

  async unfollow(evt, person: Person) {
    evt.stopPropagation();
    this.peopleService.unfollow(person.personId)
      .subscribe(() => person.isFollowing = false);
  }
}
