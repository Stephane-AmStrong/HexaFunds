import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { checkingAccountResolver } from './checking-account.resolver';

describe('checkingAccountResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => checkingAccountResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
