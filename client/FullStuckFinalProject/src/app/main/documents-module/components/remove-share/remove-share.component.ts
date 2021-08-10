import { Component, Input, OnInit } from '@angular/core';
import { DocumentProp } from 'src/app/document-prop.service';

@Component({
  selector: 'app-remove-share',
  templateUrl: './remove-share.component.html',
  styleUrls: ['./remove-share.component.css']
})
export class RemoveShareComponent implements OnInit {

  @Input() documentID!: string
  @Input() userShareID: string = ''
  @Input() documentIndex! : number
  @Input() userIndex! : number
  

  constructor(private docProp: DocumentProp) { }

  RemoveShare():void{
    this.docProp.RemoveShare(this.userIndex, this.documentIndex);
  }
  
  ngOnInit(): void { }

}
