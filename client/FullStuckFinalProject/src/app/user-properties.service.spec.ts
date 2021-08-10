import { TestBed } from '@angular/core/testing';
import { UserProperties } from './user-properties.service';

Describe('UserPropertiesService', () => {
  let service: UserProperties;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserProperties);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
