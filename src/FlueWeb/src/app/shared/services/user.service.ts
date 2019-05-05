import { Injectable, forwardRef, Inject } from '@angular/core';

import { User } from '../models';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';


@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

    public getAll(): Observable<User[]> {
        return this.http.get<User[]>(environment.accountApiUrl + `users`, {
            headers: this.authenticationService.currentUserHeader
        });
    }

    public follow(userId: string): Observable<any> {
        return this.http.post(environment.accountApiUrl + `users/Follow/${userId}`, {}, {
            headers: this.authenticationService.currentUserHeader
        });
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
