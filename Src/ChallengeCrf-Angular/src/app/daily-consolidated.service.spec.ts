import { TestBed } from '@angular/core/testing';

import { DailyConsolidatedService } from './daily-consolidated.service';

describe('DailyConsolidatedService', () => {
  let service: DailyConsolidatedService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DailyConsolidatedService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
