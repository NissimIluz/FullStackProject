import { Component, OnInit } from '@angular/core';
import { UserProperties } from '../user-properties.service';

@Component({
  selector: 'app-head-line',
  templateUrl: './head-line.component.html',
  styleUrls: ['./head-line.component.css']
})
export class HeadLineComponent implements OnInit {
  constructor(private userProp: UserProperties) {
  }
  get UserName() {
    return this.userProp.userName
  }
  ngOnInit(): void { }
}
