import { Component, OnInit } from '@angular/core';
import { DocumentProp } from 'src/app/document-prop.service';
import { UserService } from 'src/app/user/services/user.service';

@Component({
  selector: 'app-unsubscribe',
  templateUrl: './unsubscribe.component.html',
  styleUrls: ['./unsubscribe.component.css']
})
export class UnsubscribeComponent implements OnInit {

  constructor(private userService: UserService, private docProp: DocumentProp) { }

  Unsubscribe(): void {
    this.userService.Unsubscribe();
    this.docProp.Reset()
  }

  ngOnInit(): void {
  }

}
