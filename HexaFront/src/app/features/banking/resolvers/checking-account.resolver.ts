import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { CheckingAccountResponse } from '../models/checking-account-response';
import { inject } from '@angular/core';
import { CheckingAccountService } from '../services/checking-account.service';
import { Observable } from 'rxjs';

export const checkingAccountResolver: ResolveFn<CheckingAccountResponse> = (
  route: ActivatedRouteSnapshot, state: RouterStateSnapshot
): Observable<CheckingAccountResponse> => {
  const checkingAccountService = inject(CheckingAccountService);
  const id = route.params['id'];
  return checkingAccountService.getById(id!);
};
