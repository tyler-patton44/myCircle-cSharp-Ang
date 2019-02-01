import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { chatComponent } from './chat.component';

describe('chatComponent', () => {
  let component: chatComponent;
  let fixture: ComponentFixture<chatComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ chatComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(chatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
