import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { User } from '../shared/models';
import { UserCommand } from '../shared/models/commands';


@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }


    public getById(id: number) {
        return this.http.get(environment.identityApiUrl + `identify/` + id);
    }

    public register(cmd: UserCommand) {
        return this.http.post(environment.identityApiUrl + `identify/register`, cmd);
    }

    public update(user: User) {
        return this.http.put(environment.identityApiUrl + `identify/` + user.userId, user);
    }

    public delete(id: number) {
        return this.http.delete(environment.identityApiUrl + `identify/` + id);
    }
}
