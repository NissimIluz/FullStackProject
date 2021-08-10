import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { DocumentProp } from 'src/app/document-prop.service';
import { UserProperties } from 'src/app/user-properties.service';
import { WebSocketService } from 'src/app/web-socket.service';
import { CanvasCommService } from './canvas-comm.service';
import { VisualDashboardMarkersService } from './visual-dashboard-markers.service';

@Injectable({
  providedIn: 'root'
})
export class MarkerService {
  shape: string = "circle"
  debounceTime: number = 100
  strokeColor!: FormControl;
  fillColor!: FormControl
  markers: any = []
  markers$ = new Subject<any>();
  currentDocument!: string
  userID!: string
  currentlyEdited: any
  markersToPublish!: any
  constructor(private docService: DocumentProp, private canvasComm: CanvasCommService, private userProp: UserProperties, private visualService: VisualDashboardMarkersService, private webSocket: WebSocketService) { }
  /*
   * Initialize Functions
   */
  InitMarkersService(strokeColor: FormControl, fillColor: FormControl): any {
    this.userID = this.userProp.userID
    this.strokeColor = strokeColor;
    this.fillColor = fillColor
    if (!this.currentDocument || this.currentDocument != this.docService.CurrentDocumentId) {
      this.currentDocument = this.docService.currentDocumentId
      this.canvasComm.GetMarkers(this.userID, this.currentDocument).subscribe(res => this.initMarkersRespond(res))
      this.currentlyEdited = new Array()
      this.markersToPublish = new Array()
      this.webSocket.Connect(this.userID + "_doc" + ',' + this.currentDocument).subscribe((data: any) => {
        var webSocketRespond: { [key: string]: any } = {
          "addMarker": (data: any) => (this.pushToMarkers(data.MarkerDTO)),
          "removeMarker": (data: any) => (this.markers = this.markers.filter((marker: any) => (marker.markerID != data.MarkerDTO.MarkerID))),
          "updateMarker": (data: any) => {
            for (let index = 0; index < this.markers.length; index++) {
              if (this.markers[index].markerID == data.MarkerDTO.MarkerID) {
                this.markers[index].markerStrokeColor = data.MarkerDTO.MarkerStrokeColor
                this.markers[index].markerFillColor = data.MarkerDTO.MarkerFillColor
                break;
              }
            }
          }
        }
        data = JSON.parse(data.data);
        webSocketRespond[data.Action](data)
        this.markers$.next(this.markers)
      })
    }
  }

  AddNewMarker(x1: number, y1: number, x2: number, y2: number) {
    var markerDTO = {
      MarkerID: "", MarkerType: this.shape, MarkerStrokeColor: this.strokeColor.value,
      MarkerFillColor: this.fillColor.value, UserID: this.userID, DocumentID: this.currentDocument,
      X1: x1, Y1: y1, X2: x2, Y2: y2
    }
    this.canvasComm.AddNewMarker(markerDTO).subscribe(res => this.addMarkerRespond(res, markerDTO), error => window.alert("communication not found"))
  }

  RemoveMarker(marker: any) {
    var body = {
      MarkerID: marker.markerID, MarkerType: marker.markerType, MarkerStrokeColor: marker.markerStrokeColor,
      MarkerFillColor: marker.markerFillColor, UserID: this.userID, DocumentID: this.currentDocument,
      X1: marker.x1, Y1: marker.y1, X2: marker.x2, Y2: marker.y2
    }
    this.canvasComm.Remove(body).subscribe(res => this.removeRespond(res, marker))
  }
  UpdateMarker(marker: any) {
    var body = {
      MarkerID: marker.markerID, MarkerType: marker.markerType, MarkerStrokeColor: marker.markerStrokeColor,
      MarkerFillColor: marker.markerFillColor, UserID: this.userID, DocumentID: this.currentDocument,
      X1: marker.x1, Y1: marker.y1, X2: marker.x2, Y2: marker.y2
    }
    this.canvasComm.UpdateMarker(body).subscribe(res => this.updateRespond(res, marker))
  }
  RegisterColorsObservables(reColor: FormControl, reFill: FormControl, marker: any) {
    reColor.valueChanges.pipe(debounceTime(this.debounceTime)).subscribe(
      (x: any) => (this.AddChange(marker, reColor.value, reFill.value)))

    reFill.valueChanges.pipe(debounceTime(this.debounceTime)).subscribe(
      (x: any) => (this.AddChange(marker, reColor.value, reFill.value)))
  }
  AddChange(marker: any, reColor: string, reFill: string) {
    if (this.currentlyEdited.length < 50) {
      var publish = true  //first change of this marker
      if (this.markersToPublish[marker.markerID])
        publish = false //not the first change of this marker
      this.currentlyEdited.push({ marker: marker, oldColor: marker.markerStrokeColor, oldFill: marker.markerFillColor, publish: publish })
      marker.markerStrokeColor = reColor;
      marker.markerFillColor = reFill;
      this.markersToPublish[marker.markerID] = marker
      this.markers$.next(this.markers)
    }
    else {
      window.alert("Plase save changes beffor adding more chnages")
    }
  }

  SaveChanges() {
    for (var key in this.markersToPublish) {
      this.UpdateMarker(this.markersToPublish[key])
    }
    this.currentlyEdited = []
    this.markersToPublish = []
  }
  CancelLastEdit() {
    var row = this.currentlyEdited.pop()
    if (row.publish)
      delete this.markersToPublish[row.marker.markerID]
    row.marker.markerStrokeColor = row.oldColor
    row.marker.markerFillColor = row.oldFill
    this.markers$.next(this.markers)
  }

  /*
   *  Functions that handle with server respons                        
   */
  private initMarkersRespond(res: any) {
    if (res.succesed) {
      this.markers = res.markersArray.sort((a: any, b: any) => {
        return this.highestPoint(a) - this.highestPoint(b)
      });
      this.visualService.Init(this.markers.length)
    }
    else {
      window.alert(res.message)
    }
    this.markers$.next(this.markers);
  }

  private addMarkerRespond(respond: any, markerDTO: any) {
    if (respond.succeed) {
      markerDTO.MarkerID = respond.message
      this.pushToMarkers(markerDTO)
      this.visualService.Init(this.markers.length)
      this.canvasComm.SentSocket(this.currentDocument, markerDTO, "addMarker").subscribe()
    }
    else {
      window.alert(respond.message)
    }
  }
  private updateRespond(res: any, marker: any): void {
    if (res.succeed) {
      marker.userID = this.userID
      this.canvasComm.SentSocket(this.currentDocument, marker, "updateMarker").subscribe()

    }
    else {
      console.error(res.message)
    }

  }
  private removeRespond(res: any, marker: any) {
    if (res.succeed) {
      this.canvasComm.SentSocket(this.currentDocument, { UserID: this.userID, MarkerID: marker.markerID }, "removeMarker").subscribe()
      if (this.currentlyEdited.length > 0) {
        delete this.markersToPublish[marker.markerID]
        this.currentlyEdited = this.currentlyEdited.filter((row: any) => row.marker.markerID != marker.markerID)
      }
      this.markers = this.markers.filter((row: any) => marker.markerID != row.markerID)  //delete marker from markers list
      this.visualService.Init(this.markers.length)
      this.markers$.next(this.markers)
    }
    else {
      window.alert(res.message)
    }

  }

  /* 
   *  supporting function for server response function
   */
  private pushToMarkers(toPush: { MarkerID: string; MarkerType: string; MarkerStrokeColor: string; MarkerFillColor: string, UserID: string; DocumentID: string; X1: number; Y1: number; X2: number; Y2: number; }) {
    var newMarker = { markerID: toPush.MarkerID, markerType: toPush.MarkerType, markerStrokeColor: toPush.MarkerStrokeColor, markerFillColor: toPush.MarkerFillColor, userID: toPush.UserID, x1: toPush.X1, y1: toPush.Y1, x2: toPush.X2, y2: toPush.Y2 }
    if (this.markers && this.markers.length > 0) {
      var left = 0;
      var right = this.markers.length - 1
      var mid
      while (left <= right) //binary sarch
      {
        mid = Math.floor((right + left) / 2)
        if (this.highestPoint(this.markers[mid]) < this.highestPoint(newMarker)) {
          left = mid + 1
        }
        else {
          right = mid - 1
        }
      }
      mid = Math.floor((right + left) / 2)
      if (!(mid == 0 && this.highestPoint(this.markers[mid]) > this.highestPoint((newMarker)))) {
        mid += 1
      }
    }
    else {
      mid = 0
    }
    this.markers.splice(mid, 0, newMarker)
  }

  /*
   * Get /set functions
   */

  GetMarkers(): any {
    return this.markers
  }
  ChangeShape(shape: string) {
    this.shape = shape
  }

  /*
   *  Comparative functions
   */
  private highestPoint(marker: any): number {
    var highestPoint: { [key: string]: any } = {
      "circle": (marker: any) => (marker.y1 - marker.y2),
      "rectangle": (marker: any) => (marker.y1)
    };
    return highestPoint[marker.markerType](marker)
  }
}




