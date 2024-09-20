import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { TransactionRequest } from '../models/transaction-request';
import { TransactionResponse } from '../models/transaction-response';
import { TransactionQuery } from '../models/transaction-query';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  private BASE_URL: string = `transactions`;
  private http = inject(HttpClient);

  get(transactionQuery : TransactionQuery): Observable<TransactionResponse[]> {

    let params = new HttpParams();

    if (transactionQuery.withAccountId) {
      params = params.set('withAccountId', transactionQuery.withAccountId);
    }
    if (transactionQuery.fromDate) {
      params = params.set('fromDate', transactionQuery.fromDate.toISOString());
    }
    if (transactionQuery.toDate) {
      params = params.set('toDate', transactionQuery.toDate.toISOString());
    }

    return this.http
      .get<TransactionResponse[]>(this.BASE_URL,{
        params
      })
      .pipe(shareReplay());
  }

  getById(id: string): Observable<TransactionResponse> {
    return this.http
      .get<TransactionResponse>(`${this.BASE_URL}/${id}`)
      .pipe(shareReplay());
  }

  create(transaction: TransactionRequest): Observable<TransactionResponse> {
    return this.http
      .post<TransactionResponse>(this.BASE_URL, transaction)
      .pipe(shareReplay());
  }
}
