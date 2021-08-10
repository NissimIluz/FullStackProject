import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { fromEvent, Observable } from 'rxjs';
import { debounceTime, take } from 'rxjs/operators';
import { MarkerService } from '../../services/marker.service';
import { VisualDashboardMarkersService } from '../../services/visual-dashboard-markers.service';

@Component({
  selector: 'app-marker-info',
  templateUrl: './marker-info.component.html',
  styleUrls: ['./marker-info.component.css']
})
export class MarkerInfoComponent implements OnInit {
  reColor:any
  reFill!:any
  @Input() index!: number
  marker: any
  ChangeColor$! : Observable<any>
  @ViewChild("shape") shape!:ElementRef<any>
  @ViewChild("confirmColorSelect") confirmColorSelect!:ElementRef<any>
  @ViewChild("opend") opend!:ElementRef<any>
  @ViewChild("removeMarker") removeMarker!:ElementRef<any>
  removeMarker$! : Observable<any>
  inEdited: boolean =false
 
  constructor(private markerService: MarkerService, private visualService: VisualDashboardMarkersService ) {        
   }
  ngOnInit(): void {
    this.marker  = this.markerService.GetMarkers()[this.index]
    this.reColor = new FormControl(this.marker.markerStrokeColor)
    this.reFill = new FormControl(this.marker.markerFillColor)
    this.ChangeColor$= this.reColor.valueChanges
    this.ChangeColor$.pipe(take(1)).subscribe(x => this.inEdited = true)
    this.markerService.RegisterColorsObservables(this.reColor,this.reFill, this.marker)
  }
  ngAfterViewInit()
  {
    this.SetShape()
    this.removeMarker$ = fromEvent(this.removeMarker.nativeElement,"click")
    this.removeMarker$.pipe(take(1)).subscribe((evt:MouseEvent)=> this.RemoveMarker())
  }
  RemoveMarker()
  {
    this.markerService.RemoveMarker(this.marker)
  }
  Open() :boolean []
  {
    return this.visualService.open
  }
  OpenDiv()
  { 
    this.visualService.OpenDiv(this.index)
  }
  SetShape()
  {
    this.shape.nativeElement.classList.add(this.marker.markerType)   
  }
}
