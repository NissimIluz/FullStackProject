import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardMarkersComponent } from './components/dashboard-markers/dashboard-markers.component';

const routes: Routes = [
  { path: "", component: DashboardMarkersComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CanvasAreaRoutingModule { }
