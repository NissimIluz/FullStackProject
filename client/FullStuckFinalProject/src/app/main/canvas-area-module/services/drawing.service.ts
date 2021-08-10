import { ElementRef, Injectable } from '@angular/core';
import { fromEvent, Observable } from 'rxjs';
import { buffer, debounceTime, map, switchMap, takeUntil } from 'rxjs/operators'
import { GeometryService } from './geometry.service';

@Injectable({
  providedIn: 'root'
})
export class DrawingService {
  container!: ElementRef<any>
  mouseDown$!: Observable<any>
  mouseMove$!: Observable<any>
  mouseUp$!: Observable<any>
  drag$!: Observable<any>
  clientRect: any

  cxLineWidth = 2
  ctxGlobalAlpha = 0.4
  cxGlobalAlpha = 0.7
  ctxLineWidth = 3

  constructor(private geometryService: GeometryService) { }

  Init() {
    this.mouseDown$ = fromEvent(this.container.nativeElement, 'mousedown')
    this.mouseMove$ = fromEvent(this.container.nativeElement, 'mousemove')
    this.mouseUp$ = fromEvent(this.container.nativeElement, 'mouseup')
    this.drag$ = this.mouseDown$.pipe(switchMap(() => this.mouseMove$.pipe(takeUntil(this.mouseUp$))))
    this.drag$ = this.drag$.pipe(map((evt: MouseEvent) => ({
      x: evt.clientX - this.container.nativeElement.getBoundingClientRect().left,
      y: evt.clientY - this.container.nativeElement.getBoundingClientRect().y
    })))

  }
  InitSize(container: ElementRef<any>, freeDrawCanvas: ElementRef<any>, canvasContainer: ElementRef<any>) {
    this.container = container
    this.clientRect = this.container.nativeElement.getBoundingClientRect()
    freeDrawCanvas.nativeElement.left = this.clientRect.left
    freeDrawCanvas.nativeElement.height = this.clientRect.height
    freeDrawCanvas.nativeElement.width = this.clientRect.width
    freeDrawCanvas.nativeElement.top = this.clientRect.top

    canvasContainer.nativeElement.left = this.clientRect.left
    canvasContainer.nativeElement.height = this.clientRect.height
    canvasContainer.nativeElement.width = this.clientRect.width
    canvasContainer.nativeElement.top = this.clientRect.top
  }
  MouseDown(): Observable<any> {
    var retval$ = this.mouseDown$.pipe(map((evt: MouseEvent) => ({
      x: evt.clientX - this.container.nativeElement.getBoundingClientRect().left,
      y: evt.clientY - this.container.nativeElement.getBoundingClientRect().y
    })))
    return retval$
  }
  FreeDraw(): Observable<any> {
    return this.drag$
  }
  MouseUp(): Observable<any> {
    return this.mouseUp$.pipe(debounceTime(10))
  }

  DrawShape() {
    return this.drag$.pipe(buffer(this.mouseUp$), map((data) => { return data.length > 5 ? this.geometryService.GetShape(data) : undefined }))
  }
}
