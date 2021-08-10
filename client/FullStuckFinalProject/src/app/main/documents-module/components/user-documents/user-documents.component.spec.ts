import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDocumentsComponent } from './user-documents.component';

Describe('UserDocumentsComponent', () => {
  let component: UserDocumentsComponent;
  let fixture: ComponentFixture<UserDocumentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserDocumentsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
