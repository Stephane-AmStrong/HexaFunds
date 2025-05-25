import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { savingsAccountResolver } from './savings-account.resolver';

describe('savingsAccountResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => savingsAccountResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
