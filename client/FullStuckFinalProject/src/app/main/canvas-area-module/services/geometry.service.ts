import { Injectable } from '@angular/core';
import { MarkerService } from './marker.service';

@Injectable({
  providedIn: 'root'
})
export class GeometryService {

  max = 1500
  getShapeCoordinates: { [key: string]: any } = {
    "circle": (avgX: number, avgY: number, minX: number, maxX: number, minY: number, maxY: number) => ({ x1: avgX, y1: avgY, x2: Math.round((maxX - minX) / 2), y2: Math.round((maxY - minY) / 2) }),
    "rectangle": (avgX: number, avgY: number, minX: number, maxX: number, minY: number, maxY: number) => ({ x1: minX, y1: minY, x2: maxX, y2: maxY })
  };
  constructor(private markerService: MarkerService) { }

  GetShape(data: any) {
    var sumX = 0, sumY = 0
    var maxX = 0, maxY = 0
    var minX = this.max, minY = this.max

    data.forEach((row: any) => {
      sumX += row.x
      sumY += row.y

      if (row.x > maxX)
        maxX = row.x
      if (row.y > maxY)
        maxY = row.y
      if (row.x < minX)
        minX = row.x
      if (row.y < minY)
        minY = row.y
    });
    var avgX = Math.round(sumX / data.length)
    var avgY = Math.round(sumY / data.length)
    return this.getShapeCoordinates[this.markerService.shape](avgX, avgY, Math.round(minX), Math.round(maxX), Math.round(minY), Math.round(maxY))
  }
}