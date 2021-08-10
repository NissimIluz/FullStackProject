import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class UserProperties {
  userID : string = ""
  userName : string = ""

  constructor(private router:Router) {
    if(this.userID!=""){
      this.router.navigate(["main/documents"])
    }
    else
    {
      this.router.navigate([""])
    }
  }
  SetUserProperties(userID : string , userName : string) : void 
  {
    this.userID = userID
    this.userName = userName
  }

  Logout():void{
    this.userID = ""
    this.userName = ""
  }
  
  get UserID(): string {return this.userID}
  get UserName(): string {return this.userName}
}
