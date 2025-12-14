import { TestBed } from '@angular/core/testing';

import { HostService } from './host.service';
import { provideZonelessChangeDetection } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('HostService', () => {
  let service: HostService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        HostService,
        BaseApiService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection(),
      ],
    });
    service = TestBed.inject(HostService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
