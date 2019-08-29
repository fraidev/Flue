import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from '.';
import { environment } from '../../environments/environment';
import { post } from 'selenium-webdriver/http';

@Injectable({ providedIn: 'root' })
export class FeedService {

  constructor(private http: HttpClient, private authenticationService: AuthenticationService) {
  }

  public getMyFeed(): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/Feed/`, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public getPostById(postId: string): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/` + postId, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public getMyPosts(): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/MyPosts/`, {
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

  public removeComment(postId: string, commentId: string): Observable<any> {
    const cmd = {
      postId: postId,
      commentId: commentId
    };

    return this.http.post(environment.feedApiUrl + `posts/RemoveComment/`, cmd, {
      headers: this.authenticationService.currentUserHeader
    });
  }

  public GetMyPostCount(): Observable<number> {
    return this.http.get<number>(environment.feedApiUrl + `posts/MyPostCount`, {
      headers: this.authenticationService.currentUserHeader
    });
  }
}