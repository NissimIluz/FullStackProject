import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserService } from '../../services/user.service';

import { UserComponent } from './user.component';

Describe('UserComponent', () => {
  let component: UserComponent;
  let fixture: ComponentFixture<UserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(UserComponent).toBeTruthy();
    
  });
  it('should create', () => {
    expect(UserService).toBeTruthy();
});

});