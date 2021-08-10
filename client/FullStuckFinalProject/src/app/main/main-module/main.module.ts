import { NgModule } from '@angular/core';
import {MatDialogModule} from '@angular/material/dialog';
import { MainRoutingModule } from './main-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button'; 
import { ShareDocumentComponent } from '../documents-module/components/share-document/share-document.component';
import { RemoveShareComponent } from '../documents-module/components/remove-share/remove-share.component';

@NgModule({
  declarations: [
    ShareDocumentComponent,
    RemoveShareComponent
  ],
  imports: [
    CommonModule,
    MainRoutingModule,
    MatDialogModule,
    MatSelectModule,
    ReactiveFormsModule ,
    MatSelectModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatAutocompleteModule,
    MatButtonModule
  ],
  exports:
  [
    ShareDocumentComponent,
    RemoveShareComponent
  ]
})
export class MainModule { }