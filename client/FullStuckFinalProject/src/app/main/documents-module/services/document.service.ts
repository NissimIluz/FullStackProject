import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserProperties } from 'src/app/user-properties.service';
import { PeriodicElement } from 'src/app/models/document';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) }
  documents!: string[];
  serverDTO: string = "";
  userDocuments!: PeriodicElement;
  constructor(private httpClient: HttpClient, private userProp: UserProperties) { }

  SentSocket(id: string, body: any, action: string) {
    var postData = this.httpClient.post("/api/Sender/Send?content-type=application/json", {
      ID: id, MessageBody: { Action: action, DocumentDTO: body }
    }, this.httpOptions)
    return postData
  }
  GetUserDocuments() {
    let data$: Observable<any>;
    data$ = this.httpClient.post("/api/Document/GetUserDocuments/", { ID: this.userProp.UserID }, this.httpOptions);
    return data$
  }
  Displayfile(docId: string) {
    let data$: Observable<any>;
    data$ = this.httpClient.post("/api/Document/Download/", { ID: docId }, this.httpOptions);
    return data$
  }
  RemoveDocument(document: any, userId: string) {

    this.httpClient.post("/api/Document/RemoveDocument", { DocumentID: document.documentID, UserID: userId }, this.httpOptions).subscribe(
      (response: any) => {
        if (!response.succeed) {
          window.alert(response.message);
        }
        document.sharedWithUsers.forEach((user: string) => {
          this.SentSocket(user, { documentID: document.documentID }, "removeOtherUserShare").subscribe()
        });
      })
  }
  AddDocument(file: File): Observable<any> {
    const formData = new FormData();
    formData.append("file", file);
    formData.append("UserID", this.userProp.UserID);
    return this.httpClient.post("/api/Document/Upload", formData).pipe(
      map((res: any) => res.message.split(',')),
      map((arr) => ({ documentID: arr[0].split(':')[1], documentName: arr[1].split(':')[1], sharedWithUsers: [] }))
    )
  }
  RemoveShare(toShareUserID: string, documentID: string) {
    var httpOptions = {
      headers: new HttpHeaders(
        { 'Content-Type': 'application/json' }
      )
    }
    var body = { "UserID": this.userProp.UserID, "ToShareUserID": toShareUserID == '' ? this.userProp.userID : toShareUserID, "DocumentID": documentID };
    var postData = this.httpClient.post("/api/Sharing/RemoverShare", body, httpOptions);
    return postData;
  }
  get userId() {
    return this.userProp.userID;
  }
}
