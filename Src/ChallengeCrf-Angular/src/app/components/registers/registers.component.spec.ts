import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistersComponent } from './registers.component';

describe('RegistersComponent', () => {
  let component: RegistersComponent;
  let fixture: ComponentFixture<RegistersComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RegistersComponent]
    });
    fixture = TestBed.createComponent(RegistersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
