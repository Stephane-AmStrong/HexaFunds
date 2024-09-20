import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { checkingAccountResolver } from './checking-account.resolver';
import { CheckingAccountResponse } from '../models/checking-account-response';

describe('checkingAccountResolver', () => {
  const executeResolver: ResolveFn<CheckingAccountResponse> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => checkingAccountResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
