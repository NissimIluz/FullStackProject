import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DocumentsRoutingModule } from './documents-routing.module';
import { UserDocumentsComponent } from './components/user-documents/user-documents.component';
import { MainModule } from '../main-module/main.module';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatTabsModule } from '@angular/material/tabs';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [
    UserDocumentsComponent
  ],
  imports: [
    CommonModule,
    DocumentsRoutingModule,
    MainModule,
    MatTableModule,
    MatIconModule,
    MatTabsModule,
    MatListModule,
    MatButtonModule
  ]
})
export class DocumentsModule { }