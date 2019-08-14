import { Injectable } from '@angular/core';

import { User } from '../models';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';


@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }


    public getById(id: number) {
        return this.http.get(environment.identityApiUrl + `identify/` + id);
    }

    public register(user: User) {
        return this.http.post(environment.identityApiUrl + `identify/register`, user);
    }

    public update(user: User) {
        return this.http.put(environment.identityApiUrl + `identify/` + user.userId, user);
    }

    public delete(id: number) {
        return this.http.delete(environment.identityApiUrl + `identify/` + id);
    }
}
