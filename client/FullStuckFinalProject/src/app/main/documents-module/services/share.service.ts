import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ShareDocumentComponent } from '../components/share-document/share-document.component';


@Injectable({
  providedIn: 'root'
})
export class ShareService {
  users: string[] = [];
  loading: boolean = false;
  selectedUser: string = '';
  constructor(private httpClient: HttpClient, private dialog: MatDialog) { }

  OpenShareDialog(userList: string[], docId: string, userId: string): Observable<any> {
    const dialogRef = this.dialog.open(ShareDocumentComponent, {
      disableClose: true,
      panelClass: 'dialog-container-custom',
      data: {
        users: userList
      }
    });
    return dialogRef.afterClosed();
  }
  AddShare(sharedUserId: string, docId: string, userId: string) {
    var newShare = { ToShareUserID: sharedUserId, documentId: docId, userId: userId }
    var httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) }
    var data$ = this.httpClient.post("/api/Sharing/CreateShare", newShare, httpOptions);
    return data$;
  }

  get SelectedUser(): string { return this.selectedUser }
}
