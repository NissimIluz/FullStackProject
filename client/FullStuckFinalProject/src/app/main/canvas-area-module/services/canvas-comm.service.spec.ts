import { TestBed } from '@angular/core/testing';

import { CanvasCommService } from './canvas-comm.service';

Describe('CanvasCommService', () => {
  let service: CanvasCommService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CanvasCommService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
