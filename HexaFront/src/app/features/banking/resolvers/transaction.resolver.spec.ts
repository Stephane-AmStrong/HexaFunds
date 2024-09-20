import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { transactionResolver } from './transaction.resolver';
import { TransactionResponse } from '../models/transaction-response';

describe('transactionResolver', () => {
  const executeResolver: ResolveFn<TransactionResponse> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => transactionResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
