import { TestBed } from '@angular/core/testing';
import { HttpInterceptorFn } from '@angular/common/http';

import { httpInterceptor } from './http.interceptor';
import { provideZonelessChangeDetection } from '@angular/core';

describe('httpInterceptor', () => {
  const interceptor: HttpInterceptorFn = (req, next) => 
    TestBed.runInInjectionContext(() => httpInterceptor(req, next));

  beforeEach(() => {
    TestBed.configureTestingModule({providers: [provideZonelessChangeDetection()]});
  });

  it('should be created', () => {
    expect(interceptor).toBeTruthy();
  });
});
