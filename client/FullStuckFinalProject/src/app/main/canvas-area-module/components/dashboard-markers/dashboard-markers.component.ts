import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MarkerService } from '../../services/marker.service';

@Component({
  selector: 'app-dashboard-markers',
  templateUrl: './dashboard-markers.component.html',
  styleUrls: ['./dashboard-markers.component.css']
})
export class DashboardMarkersComponent implements OnInit {
  strokeColor = new FormControl('#000000')
  FillColor = new FormControl('transparent')
  fill = false

  constructor(private markerService: MarkerService) { }

  ngOnInit(): void {
    this.markerService.InitMarkersService(this.strokeColor, this.FillColor)
  }

  ngAfterViewInit(): void {

  }
  
  GetMarkers() {
    return this.markerService.GetMarkers()
  }

  ChangShape(shape: string) {
    this.markerService.ChangeShape(shape)
  }

  AllowFill() {
    this.FillColor = new FormControl('transparent')
    this.markerService.fillColor = this.FillColor
  }

  CancelLastEdit() {
    this.markerService.CancelLastEdit()
  }

  SaveChanges() {
    this.markerService.SaveChanges()
  }

  UnsavedChanges() {
    return this.markerService.currentlyEdited.length == 0
  }

  GetShape() {
    return this.markerService.shape
  }

}

