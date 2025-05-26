import {
  ActivatedRouteSnapshot,
  ResolveFn,
  RouterStateSnapshot,
} from '@angular/router';
import { SavingsAccountResponse } from '../models/savings-account-response';
import { inject } from '@angular/core';
import { SavingsAccountService } from '../services/savings-account.service';
import { Observable } from 'rxjs';

export const savingsAccountResolver: ResolveFn<SavingsAccountResponse> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): Observable<SavingsAccountResponse> => {
  const savingsAccountService = inject(SavingsAccountService);
  const id = route.params['id'];
  return savingsAccountService.getById(id!);
};
