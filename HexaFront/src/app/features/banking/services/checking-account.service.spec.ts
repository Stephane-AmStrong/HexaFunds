import { TestBed } from '@angular/core/testing';

import { CheckingAccountService } from './checking-account.service';

describe('CheckingAccountService', () => {
  let service: CheckingAccountService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CheckingAccountService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
