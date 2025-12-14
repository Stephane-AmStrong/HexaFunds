import { TestBed } from '@angular/core/testing';

import { ConnectionService } from './connection.service';
import { provideZonelessChangeDetection } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('ConnectionService', () => {
  let service: ConnectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ConnectionService,
        BaseApiService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection(),
      ],
    });
    service = TestBed.inject(ConnectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
