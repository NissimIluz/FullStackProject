import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  { path: "canvas", loadChildren: () => import('../canvas-area-module/canvas-area.module').then(m => m.CanvasAreaModule) },
  { path: "documents", loadChildren: () => import('../documents-module/documents.module').then(m => m.DocumentsModule) }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule { }