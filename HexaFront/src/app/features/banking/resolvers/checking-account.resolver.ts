import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { CheckingAccountResponse } from '../models/checking-account-response';
import { inject } from '@angular/core';
import { CheckingAccountsService } from '../services/checking-account.service';
import { Observable } from 'rxjs';

export const checkingAccountResolver: ResolveFn<CheckingAccountResponse> = (
  route: ActivatedRouteSnapshot, state: RouterStateSnapshot
): Observable<CheckingAccountResponse> => {
  const checkingAccountsService = inject(CheckingAccountsService);
  const id = route.params['id'];
  return checkingAccountsService.getById(id!);
};
