import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveShareComponent } from './remove-share.component';

Describe('RemoveShareComponent', () => {
  let component: RemoveShareComponent;
  let fixture: ComponentFixture<RemoveShareComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RemoveShareComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RemoveShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
