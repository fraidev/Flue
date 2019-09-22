import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, from } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { User } from '../shared/models';
import { Storage } from '@ionic/storage';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;

    constructor(private http: HttpClient,
        public storage: Storage) {
        this.storage.get('currentUser').then((user: User) => {
            this.currentUserSubject = new BehaviorSubject<User>(user);
            this.currentUser = this.currentUserSubject.asObservable();
        });
    }

    public get isLoggedIn() {
        return this.storage.get('currentUser').then((user) => {
            if (user) {
                // logged in so return true
                return true;
            }
            return false;
        });
    }

    public get currentUserValue(): User {
        if (this.currentUserSubject && this.currentUserSubject.value) {
            return this.currentUserSubject.value;
        } else {
            from(this.storage.get('currentUser')).subscribe((user: User) => {
                this.currentUserSubject = new BehaviorSubject<User>(user);
                this.currentUser = this.currentUserSubject.asObservable();
                return user;
            });
        }
    }

    public get currentUserHeader(): HttpHeaders {
        return new HttpHeaders({ Authorization: 'Bearer ' + this.currentUserValue.token });
    }

    public login(username: string, password: string) {
        const cmd = new User();
        cmd.username = username;
        cmd.password = password;
        return this.http.post<any>(environment.identityApiUrl + `identify/authenticate`, cmd)
            .pipe(map(user => {
                // login successful if there's a jwt token in the response
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    // localStorage.setItem('currentUser', JSON.stringify(user));
                    this.storage.set('currentUser', user);
                    this.currentUserSubject.next(user);
                }

                return user;
            }));
    }

    public logout() {
        // remove user from local storage to log user out
        return this.storage.remove('currentUser').then(() =>
            this.currentUserSubject.next(null)
        );
    }
}
