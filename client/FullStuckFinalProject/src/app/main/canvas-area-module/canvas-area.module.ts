import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CanvasAreaRoutingModule } from './canvas-area-routing.module';
import { MarkerInfoComponent } from './components/marker-info/marker-info.component';
import { CanvasAreaComponent } from './components/canvas-area/canvas-area.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTreeModule } from '@angular/material/tree';
import { MatExpansionModule } from '@angular/material/expansion';
import { ReactiveFormsModule } from '@angular/forms';
import { DashboardMarkersComponent } from './components/dashboard-markers/dashboard-markers.component';



@NgModule({
  declarations: [
    MarkerInfoComponent,
    CanvasAreaComponent,
    DashboardMarkersComponent
  ],
  imports: [
    CommonModule,
    CanvasAreaRoutingModule,
    MatTabsModule,
    MatTreeModule,
    MatExpansionModule,
    ReactiveFormsModule,
  ]
})
export class CanvasAreaModule { }
