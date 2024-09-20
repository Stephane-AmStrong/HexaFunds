import {
  ActivatedRouteSnapshot,
  ResolveFn,
  RouterStateSnapshot,
} from '@angular/router';
import { TransactionResponse } from '../models/transaction-response';
import { TransactionQuery } from '../models/transaction-query';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { TransactionService } from '../services/transaction.service';

export const transactionResolver: ResolveFn<TransactionResponse> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): Observable<TransactionResponse> => {
  const transactionService = inject(TransactionService);
  const id = route.params['id'];
  return transactionService.getById(id);
};

export const transactionsOfAccountResolver: ResolveFn<TransactionResponse[]> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): Observable<TransactionResponse[]> => {
  const transactionService = inject(TransactionService);

  const accountId: string = route.params['id'];

  const query: TransactionQuery = {
    withAccountId: accountId,
  };

  return transactionService.get(query);
};
