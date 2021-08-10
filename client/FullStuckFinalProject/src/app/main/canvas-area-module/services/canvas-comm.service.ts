import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CanvasCommService {
  
  constructor(private httpClient:HttpClient) { }
  httpOptions = {
    headers:new HttpHeaders(
      {'Content-Type':'application/json'})
  }

  SentSocket(id: string, body:any, action :string) {
    var httpOptions = { headers:new HttpHeaders({'Content-Type':'application/json'})} 
    var postData = this.httpClient.post("/api/Sender/Broadcast?content-type=application/json",{
      ID:id,MessageBody:{Action:action,MarkerDTO:body}},httpOptions)
      return postData
  }
  
  GetMarkers(userID:string, currentDocument:string)
  {
    var body = {"UserID": userID, "DocumentID": currentDocument};
    var postData= this.httpClient.post("/api/Marker/GetMarkers", body, this.httpOptions);
    return postData
  }
  AddNewMarker(markerDTO: { MarkerID: string; MarkerType: string; MarkerStrokeColor:string; MarkerFillColor: string; UserID: string; DocumentID: string; X1: number; Y1: number; X2: number; Y2: number; })
   {
    var postData= this.httpClient.post("/api/Marker/AddNewMarker", markerDTO, this.httpOptions);
    return postData
  }
  Remove(marker: any) {
    return this.httpClient.post("/api/Marker/RemoveMarker", marker, this.httpOptions);
  }
  UpdateMarker(marker: any) {
    return this.httpClient.post("/api/Marker/UpdateMarker", marker, this.httpOptions);
  }
}
