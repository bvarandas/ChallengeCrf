import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyConsolidatedComponent } from './daily-consolidated.component';

describe('DailyConsolidatedComponent', () => {
  let component: DailyConsolidatedComponent;
  let fixture: ComponentFixture<DailyConsolidatedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DailyConsolidatedComponent]
    });
    fixture = TestBed.createComponent(DailyConsolidatedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
