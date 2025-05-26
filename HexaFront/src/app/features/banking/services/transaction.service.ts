import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { TransactionRequest } from '../models/transaction-request';
import { TransactionResponse } from '../models/transaction-response';
import { TransactionQuery } from '../models/transaction-query';
import { HttpWrapperService } from './http-wrapper.service';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  private BASE_URL: string = `transactions`;
  private http = inject(HttpWrapperService);

  getAll(): Observable<TransactionResponse[]> {
    return this.http.handleRequest('GET', this.BASE_URL);
  }

  get(transactionQuery: TransactionQuery): Observable<TransactionResponse[]> {
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

    return this.http.handleRequest('GET', this.BASE_URL, {
      params,
    });
  }

  getById(id: string): Observable<TransactionResponse> {
    return this.http.handleRequest('GET', `${this.BASE_URL}/${id}`);
  }

  create(transaction: TransactionRequest): Observable<TransactionResponse> {
    return this.http.handleRequest('POST', this.BASE_URL, {
      body: transaction,
    });
  }
}
