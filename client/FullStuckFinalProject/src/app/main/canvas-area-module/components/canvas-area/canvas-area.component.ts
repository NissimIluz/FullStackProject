import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DrawingService } from '../../services/drawing.service';
import { DocumentProp } from 'src/app/document-prop.service';
import { DomSanitizer } from '@angular/platform-browser';
import { MarkerService } from '../../services/marker.service';

@Component({
  selector: 'app-canvas-area',
  templateUrl: './canvas-area.component.html',
  styleUrls: ['./canvas-area.component.css']
})

export class CanvasAreaComponent implements OnInit {
  private cx!: CanvasRenderingContext2D;
  private ctx!: CanvasRenderingContext2D;
  private cxSelected!: CanvasRenderingContext2D;
  @ViewChild("canvasContainer") canvasContainer!:ElementRef<any>
  @ViewChild("canvasSelected") canvasSelected!:ElementRef<any>
  @ViewChild("CanvasFreeDrew") CanvasFreeDrew!:ElementRef<any>
  @ViewChild("container") container!:ElementRef<any>
  prevPos: { x: number, y: number } ={x:0,y:0}
  documentId:string = '';
  drawingShape : { [key: string]: any } = {
    "circle": (object:any)=>this.ctx.ellipse(object.x1, object.y1, object.x2, object.y2, Math.PI , 0, 2 * Math.PI),
    "rectangle": (object:any)=> this.ctx.rect(object.x1, object.y1, object.x2 - object.x1,  object.y2 - object.y1)
};
  constructor(private docProp:DocumentProp, private markerService: MarkerService, private drawingService:DrawingService, private sanitizer: DomSanitizer) {
  }
  /*
   * Initialize fonctions
   */
  ngOnInit(): void { 
    this.docProp.DisplayDocument()
   }
  
  ngAfterViewInit(): void{
    this.drawingService.InitSize(this.container, this.CanvasFreeDrew, this.canvasContainer)
    this.drawingService.Init()
    this.drawingService.MouseDown().subscribe((evt) => (this.prevPos =evt))
    this.drawingService.FreeDraw().subscribe((evt) => (this.FreeDraw(evt)))
    this.drawingService.MouseUp().subscribe((evt) => this.clear(this.cx))
    this.drawingService.DrawShape().subscribe(data =>{
        if(data){
          this.DrawShape(data,this.markerService.strokeColor.value,this.markerService.fillColor.value,this.markerService.shape);
          this.markerService.AddNewMarker(data.x1,data.y1,data.x2,data.y2)
    }})
   
    this.markerService.markers$.subscribe( (markers:any) => this.showDocumentMarkers(markers))
    this.cx = this.CanvasFreeDrew.nativeElement.getContext('2d');
    this.ctx = this.canvasContainer.nativeElement.getContext('2d');
    this.ctx.globalAlpha =this.drawingService.ctxGlobalAlpha
    this.cx.globalAlpha = this.drawingService.cxGlobalAlpha
    this.ctx.lineWidth = this.drawingService.ctxLineWidth
    this.cx.lineWidth = this.drawingService.cxLineWidth
    if(this.markerService.GetMarkers())
    {
      this.showDocumentMarkers(this.markerService.GetMarkers())
    }
  }
  getUrl(){
    return this.sanitizer.bypassSecurityTrustUrl(this.docProp.CurrentDocumentUrl);
  }
  private showDocumentMarkers(markers: any){
    this.clear(this.ctx);
    markers.forEach((marker:any) => {
      this.DrawShape({x1: marker.x1, y1:marker.y1, x2:marker.x2, y2: marker.y2}, marker.markerStrokeColor, marker.markerFillColor,marker.markerType)
    });
  }
 
  /*
   * Draw Functions
   */
  private FreeDraw(currentPos: { x: number, y: number } ) 
  {
    if (!this.cx) { return; }
    this.cx.strokeStyle = this.markerService.strokeColor.value;
    if (this.prevPos) {
      this.cx.beginPath()
      this.cx.moveTo(this.prevPos.x, this.prevPos.y); 
      this.cx.lineTo(currentPos.x, currentPos.y);
      this.cx.stroke();
      this.prevPos = currentPos
      this.cx.closePath()
    }
  }
 
  private DrawShape(data:any, color:any, fill:string, shape: string) :void 
  {   
    this.ctx.beginPath()
    this.ctx.strokeStyle=color
    this.ctx.fillStyle =fill
    this.drawingShape[shape](data)
    this.ctx.fill();
    this.ctx.stroke()
    this.ctx.closePath()
  }
  private clear(cx:CanvasRenderingContext2D)
  {
    cx.clearRect(0, 0, this.CanvasFreeDrew.nativeElement.getBoundingClientRect().width, this.CanvasFreeDrew.nativeElement.getBoundingClientRect().height)
  }
 
}


