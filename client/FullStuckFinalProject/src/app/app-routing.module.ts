import { HttpClient } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommService } from './user/services/comm.service';


const routes: Routes = [
  {path:"",loadChildren:() => import('../app/user/user.module').then(m=>m.UserModule)},
  {path:"main", loadChildren:() => import('../app/main/main-module/main.module').then(m=>m.MainModule)}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [CommService,HttpClient]
})
export class AppRoutingModule { }
