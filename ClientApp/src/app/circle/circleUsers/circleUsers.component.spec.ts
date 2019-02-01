import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { circleUsersComponent } from './circleUsers.component';

describe('circleUsersComponent', () => {
  let component: circleUsersComponent;
  let fixture: ComponentFixture<circleUsersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ circleUsersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(circleUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
