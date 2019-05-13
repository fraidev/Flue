import { Injectable } from '@angular/core';
import { Person } from '../models';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from './authentication.service';

@Injectable({ providedIn: 'root' })
export class PeopleService {
    public followers: Person[];
    public following: Person[];
    constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }
    public configure() {
        this.getFollowers().subscribe(x => this.followers = x);
        this.getFollowing().subscribe(x => this.following = x);
    }
    public cotainsFollowing(userId: string): boolean {
        return this.following.some(x => x.userId === userId);
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
