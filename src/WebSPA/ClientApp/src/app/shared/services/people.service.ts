import { Injectable } from '@angular/core';
import { Person } from '../models';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject, combineLatest, Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from './authentication.service';
import { map } from 'rxjs/operators';
import { ProfileBoxComponent } from '../../applications/feed/profile-box/profile-box.component';


@Injectable({ providedIn: 'root' })
export class PeopleService {
    public followers = new Array<Person>();
    public following = new Array<Person>();
    public mePerson = new BehaviorSubject<Person>(new Person());

    public get me(): Person {
        return this.mePerson.getValue();
    }

    constructor(private http: HttpClient, private authenticationService: AuthenticationService) {
    }

    public configure(profileBox: ProfileBoxComponent): Subscription {
        return combineLatest(this.getMe(), this.getFollowers(), this.getFollowing()).subscribe(([a, b, c]) => {
            this.mePerson = new BehaviorSubject<Person>(a);
            this.followers = b;
            this.following = c;
            profileBox.config();
        });
    }

    public getfollowingCount(): number {
        return this.following.length;
    }

    public cotainsFollowing(personId: string): boolean {
        return this.following.some(x => x.personId === personId);
    }
    public getFollowers(): Observable<Person[]> {
        return this.http.get<Person[]>(environment.feedApiUrl + `people/Followers`, {
            headers: this.authenticationService.currentUserHeader
        });
    }
    public getFollowing(): Observable<Person[]> {
        return this.http.get<Person[]>(environment.feedApiUrl + `people/Following`, {
            headers: this.authenticationService.currentUserHeader
        });
    }
    public getMe(): Observable<Person> {
        return this.http.get<Person>(environment.feedApiUrl + `people/Me`, {
            headers: this.authenticationService.currentUserHeader
        });
    }
    public getPeople(searchText: string): Observable<Person[]> {
        let params = new HttpParams();
        params = params.append('searchText', searchText);
        return this.http.get<Person[]>(environment.feedApiUrl + `people/`, {
            params,
            headers: this.authenticationService.currentUserHeader
        });
    }
    public getAll(): Observable<Person[]> {
        return this.http.get<Person[]>(environment.feedApiUrl + `people`, {
            headers: this.authenticationService.currentUserHeader
        });
    }
    public follow(userId: string) {
        return this.http.post(environment.feedApiUrl + `people/Follow/${userId}`, {}, {
            headers: this.authenticationService.currentUserHeader
        }).subscribe(z => this.getFollowing().subscribe(x => this.following = x));
    }
    public unfollow(userId: string) {
        return this.http.post(environment.feedApiUrl + `people/Unfollow/${userId}`, {}, {
            headers: this.authenticationService.currentUserHeader
        }).subscribe(z => this.getFollowing().subscribe(x => this.following = x));
    }
}
