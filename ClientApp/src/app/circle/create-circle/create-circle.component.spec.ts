import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCircleComponent } from './create-circle.component';

describe('CreateCircleComponent', () => {
  let component: CreateCircleComponent;
  let fixture: ComponentFixture<CreateCircleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateCircleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateCircleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
