import { TestBed } from '@angular/core/testing';

import { HighchartsThemeService } from './highcharts-theme.service';
import { provideZonelessChangeDetection } from '@angular/core';

describe('HighchartsThemeService', () => {
  let service: HighchartsThemeService;

  beforeEach(() => {
    TestBed.configureTestingModule({providers: [provideZonelessChangeDetection()]});
    service = TestBed.inject(HighchartsThemeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
