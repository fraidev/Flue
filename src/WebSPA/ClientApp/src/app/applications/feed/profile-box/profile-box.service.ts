import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from 'src/app/shared/services';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

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
