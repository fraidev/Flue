import { Injectable } from '@angular/core';
import { AuthenticationService } from '../../services';
import { environment } from 'src/environments/environment';
import { Post } from '../../models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProfileBoxService {

  constructor(private http: HttpClient, private authenticationService: AuthenticationService) { }

  public GetMyPostCount(): Observable<number> {
    return this.http.get<number>(environment.feedApiUrl + `posts/MyPostCount`, {
      headers: this.authenticationService.currentUserHeader
    });
  }
}
