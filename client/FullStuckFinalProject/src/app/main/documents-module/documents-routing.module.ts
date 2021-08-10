import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserDocumentsComponent } from './components/user-documents/user-documents.component';

const routes: Routes = [
  { path: "", component: UserDocumentsComponent },


];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentsRoutingModule { }
