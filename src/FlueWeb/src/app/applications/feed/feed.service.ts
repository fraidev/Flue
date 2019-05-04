import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthenticationService } from 'src/app/shared/services';

@Injectable({ providedIn: 'root' })
export class FeedService {

  constructor(private http: HttpClient, private authenticationService: AuthenticationService) {
  }

  public createPost(post: any): Observable<any> {
    return this.http.post(environment.feedApiUrl + `posts/`, post, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public getInbox(): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/Inbox/`, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public indentity(): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/Identity/`, {
      headers: this.authenticationService.currentUserHeader
    });
  }
}
