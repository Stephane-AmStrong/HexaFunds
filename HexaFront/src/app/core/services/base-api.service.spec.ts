import { TestBed } from '@angular/core/testing';

import { BaseApiService } from './base-api.service';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('BaseApiService', () => {
  let service: BaseApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [
        provideZonelessChangeDetection(),
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });
    service = TestBed.inject(BaseApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
