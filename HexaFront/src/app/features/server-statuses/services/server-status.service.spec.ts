import { TestBed } from '@angular/core/testing';

import { provideZonelessChangeDetection } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ServerStatusService } from './server-status.service';

describe('ServerStatuseservice', () => {
  let service: ServerStatusService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ServerStatusService,
        BaseApiService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection(),
      ],
    });
    service = TestBed.inject(ServerStatusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
