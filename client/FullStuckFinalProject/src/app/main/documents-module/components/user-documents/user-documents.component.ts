import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatTable } from '@angular/material/table';
import { dataTable, shareDTO } from 'src/app/models/document';
import { DocumentProp } from 'src/app/document-prop.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-documents',
  templateUrl: './user-documents.component.html',
  styleUrls: ['./user-documents.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class UserDocumentsComponent implements OnInit {
  @ViewChild('table') matTable!: MatTable<dataTable>;
  @ViewChild('fileUpload') fileUpload!: ElementRef;
  fileName = '';
  dataimage: any;
  expandedElement!: shareDTO;
  columnsToDisplay: string[] = ['documentName', 'sharedWithUsers', 'addShare', 'removeDocument'];
  dataSource: any;
  sharedDocuments: any = [];
  constructor(private docProp: DocumentProp, private router: Router) {
    this.dataSource = this.docProp.UserDocuments;
  }

  ngOnInit(): void {
    this.docProp.GetUserDocuments();
  }
  DisplayFileName() {
    var file: File = this.fileUpload.nativeElement.files[0];
    if (file) {
      this.fileName = file.name;
    }
  }
  AddDocument() {
    var file = this.fileUpload.nativeElement.files[0];
    var types = ["image/gif", "image/jpeg", "image/jpg", "image/png"];
    if (file) {
      if (types.indexOf(file.type) === -1) {
        alert("please use proper image")
      } else {
        this.docProp.AddDocument(file);
        this.matTable.renderRows();
      }
      this.fileUpload.nativeElement.value = null;
      setTimeout(() => this.fileName = '', 350);
    }
  }
  RemoveDocument(index: number) {
    this.docProp.RemoveDocument(index);
    this.matTable.renderRows();
  }
  FileOpen(docId: string) {
    this.docProp.SetCurrentDocId(docId);
    this.router.navigate(['main/canvas']);
  }
  OpenDialog(docId: string, index: number) {
    this.docProp.AddShare(docId, index);
  }
}
