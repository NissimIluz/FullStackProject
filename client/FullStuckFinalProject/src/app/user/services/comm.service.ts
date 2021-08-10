import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormGroup } from '@angular/forms';
import { UserProperties } from '../../user-properties.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommService {
  httpOptions = {
    headers:new HttpHeaders(
      {'Content-Type':'application/json'}
    )
  }
  constructor(private httpClient:HttpClient, private userProperties:UserProperties){}

  User(action:string,userFormData:FormGroup)
  {
    var postData
    if(action=="Login")
    {
          postData = this.httpClient.post("/api/User/Login/",{ID : userFormData.controls.userID.value}, this.httpOptions)
    }
    else
    {
          postData = this.httpClient.post("/api/User/SignUp", userFormData.value, this.httpOptions)
    }
    return postData
  }
  GetUsers(userID: string):Observable<any>
  {
     return this.httpClient.post("/api/User/GetUsers",{ID: userID}, this.httpOptions)
  }
  Unsubscribe(userID: string){
    var postData= this.httpClient.post("/api/User/MarkUserAsRemoved/", {ID: userID}, this.httpOptions);
    return postData
  }
}
  
  
