import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { SupportMessage } from '../shared/models/support.mesange.model';

@Injectable({ providedIn: 'root' })
export class SupportService {
    constructor(private http: HttpClient) {
    }

    public getSupportMessages(): Observable<SupportMessage[]> {
        return this.http.get<SupportMessage[]>(environment.suportApiUrl + 'supportMessages');
    }

    public createSuportMessage(text: string, userId: string) {
        const cmd = {
            text: text,
            userId: userId
        };
        return this.http.post(environment.suportApiUrl + 'createSuportMessage', cmd);
   }
}
