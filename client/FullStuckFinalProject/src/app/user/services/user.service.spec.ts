import { TestBed, tick } from '@angular/core/testing';
import { FormControl, FormGroup } from '@angular/forms';
import { UserService } from './user.service';



Describe('UserService', () => {
  let service: UserService;
  var userFormData: FormGroup


  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserService);
    userFormData = new FormGroup({
      userName: new FormControl("test1@test.co.il"),
      userID: new FormControl("test1@test.co.il")
    })
    UserService
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create', () => {
    expect(UserService).toBeTruthy();
  });
});
