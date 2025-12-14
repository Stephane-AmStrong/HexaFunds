import { TestBed } from '@angular/core/testing';

import { DashboardService } from './dashboard.service';
import { provideZonelessChangeDetection } from '@angular/core';

describe('DashboardService', () => {
  let service: DashboardService;

  beforeEach(() => {
    TestBed.configureTestingModule({providers: [provideZonelessChangeDetection()]});
    service = TestBed.inject(DashboardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
