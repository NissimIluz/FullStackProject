import { Injectable } from '@angular/core';
import { UserProperties } from 'src/app/user-properties.service';
import { DocumentService } from './main/documents-module/services/document.service';
import { ShareService } from './main/documents-module/services/share.service';
import { PeriodicElement } from './models/document';
import { CommService } from './user/services/comm.service';
import { WebSocketService } from './web-socket.service';

@Injectable({
  providedIn: 'root'
})

export class DocumentProp {
  userDocuments: PeriodicElement = {ownDocuments: [], sharedWithUser: []};
  currentDocumentId: string = '';
  currenctDocumentUrl: string = '';
  users: string[] = [];
  constructor(private docService: DocumentService, private userProp: UserProperties, private communicationService : CommService,
    private shareService: ShareService, private webSocket: WebSocketService) { }
  GetUserDocuments()
  {
    this.communicationService.GetUsers(this.userProp.userID).subscribe(
                  (response)=> {this.users = response; });
    this.webSocket.Connect(this.userProp.userID).subscribe((data:any)=>
    {
      var webSocketRespond: { [key: string]: any } = {
            "newShare":    (data:any)=> (this.userDocuments.sharedWithUser = [...this.userDocuments.sharedWithUser, data.DocumentDTO]),
            "removeOtherUserShare":(data:any)=>
            {
            this.userDocuments.sharedWithUser,
            this.userDocuments.sharedWithUser =this.userDocuments.sharedWithUser.filter((row:any) =>row.documentID ==data.documentID);
            if(this.CurrentDocumentId==data.DocumentDTO.documentID)
               window.alert("Document: "+data.DocumentDTO.documentName+"\nis no longer shared with you")
            }         
          }; 
      data=  JSON.parse(data.data);
      webSocketRespond[data.Action](data)
      })
    if(this.userDocuments.ownDocuments.length == 0){
       this.docService.GetUserDocuments().subscribe(
         (response) =>{ 
            this.userDocuments.ownDocuments = response.ownDocuments;
            this.userDocuments.sharedWithUser = response.sharedWithUser;
         });
    }

    return this.userDocuments;
  }

  RemoveDocument(index: number)
  {
    var userId = this.docService.userId;
    this.docService.RemoveDocument(this.userDocuments.ownDocuments[index], userId);
    this.userDocuments.ownDocuments.splice(index, 1);
    
  }
  AddDocument(file: File) {
    this.docService.AddDocument(file).subscribe(
      (response)=> {
          this.userDocuments.ownDocuments = [...this.userDocuments.ownDocuments, response];
      }
    );
  }
  DisplayDocument() {
    this.docService.Displayfile(this.currentDocumentId).subscribe((res) => {
      if(res.succeed)
        this.currenctDocumentUrl = res.message;
      else
        window.alert(res.message)
    });
  }
  AddShare(docId:string, index:number){
    var data$ = this.shareService.OpenShareDialog(this.users,docId,this.userProp.userID);
    data$.subscribe(result => {
      var res$ = this.shareService.AddShare(result, docId, this.userProp.userID);
      res$.subscribe(
        (response: any ) => {
          if(response.succeed){
            if(!this.userDocuments.ownDocuments[index].sharedWithUsers){
              this.userDocuments.ownDocuments[index].sharedWithUsers = [];                           
            }
            this.docService.SentSocket(result,this.userDocuments.ownDocuments[index],"newShare").subscribe()
            this.userDocuments.ownDocuments[index].sharedWithUsers =
            [...this.userDocuments.ownDocuments[index].sharedWithUsers, result]
          }
          else{
            alert(response.message)
          }
        })
    });
  }
  RemoveShare(userIndex:number, docIndex:number)
  { 
    var x= 0
    var docId:any ,  userId:string
    if(userIndex!=undefined){
      docId = this.userDocuments.ownDocuments[docIndex].documentID
      userId = this.userDocuments.ownDocuments[docIndex].sharedWithUsers[userIndex];
    }
    else {
      docId = this.userDocuments.sharedWithUser[docIndex].documentID
      userId = this.userProp.userID;
    }
    
    this.docService.RemoveShare(userId, docId).subscribe(
      (res:any) => {
        if(!res.succeed){
          window.alert(res.message);
        }
        if(userIndex!=undefined) {
          this.docService.SentSocket( userId,{documentID:docId,documentName:this.userDocuments.ownDocuments[docIndex].documentName},"removeOtherUserShare").subscribe()
          this.userDocuments.ownDocuments[docIndex].sharedWithUsers.splice(userIndex, 1)    
        } else {
          this.userDocuments.sharedWithUser.splice(docIndex, 1);
        }
      }
    )
  }
  SetCurrentDocId(docId:string){
    this.currentDocumentId = docId;
  }
  Reset()
  {
    this.userDocuments = {ownDocuments: [], sharedWithUser: []};
    this.currentDocumentId = '';
    this.currenctDocumentUrl = '';
    this.users = []; 
  }

  get CurrentDocumentUrl(): string { return this.currenctDocumentUrl }
  get UserDocuments(): PeriodicElement { return this.userDocuments}
  get CurrentDocumentId(): string { return this.currentDocumentId }
}

