import { Injectable, forwardRef, Inject } from '@angular/core';

import { User } from '../models';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';


@Injectable({ providedIn: 'root' })
export class UserService {
    public followers: User[];
    public following: User[];

    constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

    public configure() {
        this.getFollowers().subscribe(x => this.followers = x);
        this.getFollowing().subscribe(x => this.following = x);
    }

    public cotainsFollowing(userId: string): boolean {
        return this.following.some(x => x.id === userId);
    }

    public getFollowers(): Observable<User[]> {
        return this.http.get<User[]>(environment.accountApiUrl + `users/Followers`, {
            headers: this.authenticationService.currentUserHeader
        });
    }

    public getFollowing(): Observable<User[]> {
        return this.http.get<User[]>(environment.accountApiUrl + `users/Following`, {
            headers: this.authenticationService.currentUserHeader
        });
    }

    public getUsers(searchText: string): Observable<User[]> {
        let params = new HttpParams();
        params = params.append('searchText', searchText);


        return this.http.get<User[]>(environment.accountApiUrl + `users/`,  {
            params,
            headers: this.authenticationService.currentUserHeader
        });
    }

    public getAll(): Observable<User[]> {
        return this.http.get<User[]>(environment.accountApiUrl + `users`, {
            headers: this.authenticationService.currentUserHeader
        });
    }

    public follow(userId: string) {
        return this.http.post(environment.accountApiUrl + `users/Follow/${userId}`, {}, {
            headers: this.authenticationService.currentUserHeader
        }).subscribe(z => this.getFollowing().subscribe(x => this.following = x));
    }

    public unfollow(userId: string) {
        return this.http.post(environment.accountApiUrl + `users/Unfollow/${userId}`, {}, {
            headers: this.authenticationService.currentUserHeader
        }).subscribe(z => this.getFollowing().subscribe(x => this.following = x));
    }

    public getById(id: number) {
        return this.http.get(environment.accountApiUrl + `users/` + id);
    }

    public register(user: User) {
        return this.http.post(environment.accountApiUrl + `users/register`, user);
    }

    public update(user: User) {
        return this.http.put(environment.accountApiUrl + `users/` + user.id, user);
    }

    public delete(id: number) {
        return this.http.delete(environment.accountApiUrl + `users/` + id);
    }
}
