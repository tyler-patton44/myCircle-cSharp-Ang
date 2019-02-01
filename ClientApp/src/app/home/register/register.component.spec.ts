import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { registerComponent } from './register.component';

describe('registerComponent', () => {
  let component: registerComponent;
  let fixture: ComponentFixture<registerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ registerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(registerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
