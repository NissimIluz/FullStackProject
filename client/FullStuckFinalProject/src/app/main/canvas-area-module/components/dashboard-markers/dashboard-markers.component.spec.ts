import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardMarkersComponent } from './dashboard-markers.component';

Describe('DashboardMarkersComponent', () => {
  let component: DashboardMarkersComponent;
  let fixture: ComponentFixture<DashboardMarkersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardMarkersComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardMarkersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
