import { TestBed } from '@angular/core/testing';

import { CheckingAccountsService } from './checking-account.service';

describe('CheckingAccountsService', () => {
  let service: CheckingAccountsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CheckingAccountsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
