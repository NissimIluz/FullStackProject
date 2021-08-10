import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DocumentProp } from 'src/app/document-prop.service';
import { UserProperties } from 'src/app/user-properties.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private docProp: DocumentProp, private userProp: UserProperties, private routing: Router) { }

  Logout(): void {
    this.userProp.Logout();
    this.docProp.Reset();
    this.routing.navigate([""]);
  }

  ngOnInit(): void {
  }

}
