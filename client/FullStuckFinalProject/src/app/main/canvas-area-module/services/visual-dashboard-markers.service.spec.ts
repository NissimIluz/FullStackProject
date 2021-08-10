import { TestBed } from '@angular/core/testing';

import { VisualDashboardMarkersService } from './visual-dashboard-markers.service';

Describe('VisualDashboardMarkersService', () => {
  let service: VisualDashboardMarkersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VisualDashboardMarkersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
