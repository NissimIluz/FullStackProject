import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UserProperties } from 'src/app/user-properties.service';

@Injectable({
  providedIn: 'root'
})
export class UnsubscribeService {
  serverResponse: string = "";

  constructor(private userProp: UserProperties, private httpClient: HttpClient, private router: Router) { }

  Unsubscribe(): void {
    var httpOptions = {
      headers: new HttpHeaders(
        { 'Content-Type': 'application/json' }
      )
    }
    var postData = this.httpClient.put("/api/User/MarkUserAsRemoved/" + this.userProp.UserID, "", httpOptions);
    postData.subscribe((response: any) => this.ServerResponse(response.succeed, response.message),
      error => this.ServerResponse(false, error))
  }
  ServerResponse(succeed: boolean, message: string): void {
    if (succeed) {
      console.log(message)
      this.userProp.Logout();
      this.router.navigate([""])
    }
    else {
      this.serverResponse = message
    }
  }
}
