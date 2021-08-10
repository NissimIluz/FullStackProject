import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { HttpClient } from '@angular/common/http';
import { UserComponent } from './controllers/user-auth/user.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';


@NgModule({
  declarations: [
    UserComponent,

  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatSelectModule,   
  ],
  providers : [HttpClient]
})
export class UserModule { }
