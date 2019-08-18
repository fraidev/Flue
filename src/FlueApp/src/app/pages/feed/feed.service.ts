import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../../providers/services';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class FeedService {

  constructor(private http: HttpClient, private authenticationService: AuthenticationService) {
  }

  public getMyFeed(): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/Feed/`, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public createPost(post: any): Observable<any> {
    return this.http.post(environment.feedApiUrl + `posts/`, post, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public deletePost(id: string): Observable<any> {
    return this.http.delete(environment.feedApiUrl + `posts/` + id, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public addComment(cmd: any): Observable<any> {
    return this.http.post(environment.feedApiUrl + `posts/comment/`, cmd, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public removeComment(id: string): Observable<any> {
    return this.http.delete(environment.feedApiUrl + `posts/comment/` + id, {
      headers: this.authenticationService.currentUserHeader
    });
  }
}
