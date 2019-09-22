import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { AuthenticationService } from './authentication.service';
import { Person } from '../shared/models';
import { environment } from '../../environments/environment';


@Injectable({ providedIn: 'root' })
export class PeopleService {
    constructor(private http: HttpClient,
        private authenticationService: AuthenticationService) {
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
        });
    }
    public unfollow(userId: string) {
        return this.http.post(environment.feedApiUrl + `people/Unfollow/${userId}`, {}, {
            headers: this.authenticationService.currentUserHeader
        });
    }

    public updatePerson(cmd: { name: string; email: string; description: string; profilePicture: string; }) {
        return this.http.post(environment.feedApiUrl + `people/UpdatePerson/`, cmd, {
            headers: this.authenticationService.currentUserHeader
        });
    }

}
