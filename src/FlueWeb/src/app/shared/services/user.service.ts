import { Injectable } from '@angular/core';

import { User } from '../models';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';


@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(environment.accountApiUrl + `users`);
    }

    getById(id: number) {
        return this.http.get(environment.accountApiUrl + `users/` + id);
    }

    register(user: User) {
        return this.http.post(environment.accountApiUrl + `users/register`, user);
    }

    update(user: User) {
        return this.http.put(environment.accountApiUrl + `users/` + user.id, user);
    }

    delete(id: number) {
        return this.http.delete(environment.accountApiUrl + `users/` + id);
    }
}
