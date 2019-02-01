import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { inviteUserComponent } from './inviteUser.component';

describe('inviteUserComponent', () => {
  let component: inviteUserComponent;
  let fixture: ComponentFixture<inviteUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ inviteUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(inviteUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
