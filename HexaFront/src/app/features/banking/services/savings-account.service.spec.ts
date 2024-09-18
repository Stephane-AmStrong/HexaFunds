import { TestBed } from '@angular/core/testing';

import { SavingsAccountService } from './savings-account.service';

describe('SavingsAccountService', () => {
  let service: SavingsAccountService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SavingsAccountService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
