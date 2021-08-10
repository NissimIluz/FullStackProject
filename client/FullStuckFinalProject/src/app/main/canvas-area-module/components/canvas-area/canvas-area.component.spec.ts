import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CanvasAreaComponent } from './canvas-area.component';

Describe('CanvasAreaComponent', () => {
  let component: CanvasAreaComponent;
  let fixture: ComponentFixture<CanvasAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CanvasAreaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CanvasAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
