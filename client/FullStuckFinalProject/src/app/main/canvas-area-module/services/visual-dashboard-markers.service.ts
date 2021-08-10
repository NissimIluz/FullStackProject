import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class VisualDashboardMarkersService {
  open!: boolean[]
  lastOpened: number = 0
  constructor() {
  }
  Init(length: number) {
    this.open = new Array(length)
    this.open.fill(false)
  }
  Open() {
    return this.open
  }
  OpenDiv(index: number) {
    this.open[index] = !this.open[index]
    if (this.lastOpened != index) {
      this.open[this.lastOpened] = false
      this.lastOpened = index
    }
  }
}
