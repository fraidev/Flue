import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class FeedService {

  constructor(private http: HttpClient) {
  }

  public getMyFeed(page: number, itemsPerPage: number): Observable<any> {
    let params = new HttpParams();
    params = params.append('page', page.toString());
    params = params.append('itemsPerPage', itemsPerPage.toString());

    return this.http.get(environment.feedApiUrl + `posts/Feed/`, { params });
  }

  public getPostByUserId(postId: string): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/` + postId);
  }

  public getPostById(postId: string): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/` + postId);
  }

  public getPostsByPersonId(personId: string): Observable<any> {
    return this.http.get(environment.feedApiUrl + `posts/person/` + personId);
  }

  public createPost(post: any): Observable<any> {
    return this.http.post(environment.feedApiUrl + `posts/`, post);
  }

  public deletePost(id: string): Observable<any> {
    return this.http.delete(environment.feedApiUrl + `posts/` + id);
  }

  public addComment(cmd: any): Observable<any> {
    return this.http.post(environment.feedApiUrl + `posts/comment/`, cmd);
  }

  public removeComment(postId: string, commentId: string): Observable<any> {
    const cmd = {
      postId: postId,
      commentId: commentId
    };

    return this.http.post(environment.feedApiUrl + `posts/RemoveComment/`, cmd);
  }
}
