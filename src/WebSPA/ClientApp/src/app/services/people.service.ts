import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication.service';
import { Person } from '../shared/models';
import { environment } from '../../environments/environment';


@Injectable({ providedIn: 'root' })
export class PeopleService {
    constructor(private http: HttpClient) {
    }

    public getMe(): Observable<Person> {
        return this.http.get<Person>(environment.feedApiUrl + `people/Me`);
    }

    public getPeople(searchText: string, page: number, itemsPerPage: number,
        searchPeopleType?: 'All' | 'Followings' | 'Followers', personId?: string): Observable<Person[]> {
        let params = new HttpParams();
        params = params.append('searchText', searchText);
        params = params.append('page', page.toString());
        params = params.append('itemsPerPage', itemsPerPage.toString());
        if (searchPeopleType) {
            params = params.append('searchPeopleType', searchPeopleType);
        }
        if (personId) {
            params = params.append('personId', personId);
        }
        return this.http.get<Person[]>(environment.feedApiUrl + `people/`, { params });
    }

    public getAll(): Observable<Person[]> {
        return this.http.get<Person[]>(environment.feedApiUrl + `people`);
    }
    public follow(userId: string) {
        return this.http.post(environment.feedApiUrl + `people/Follow/${userId}`, {});
    }
    public unfollow(userId: string) {
        return this.http.post(environment.feedApiUrl + `people/Unfollow/${userId}`, {});
    }

    public updatePerson(cmd: { name: string; email: string; description: string; profilePicture: string; }) {
        return this.http.post(environment.feedApiUrl + `people/UpdatePerson/`, cmd);
    }

}
