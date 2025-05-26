import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, startWith, switchMap, tap } from 'rxjs';
import { CheckingAccountResponse } from '../models/checking-account-response';
import { CheckingAccountRequest } from '../models/checking-account-request';
import { HttpWrapperService } from './http-wrapper.service';

@Injectable({
  providedIn: 'root',
})
export class CheckingAccountService {
  private BASE_URL: string = `checkingaccounts`;
  private http = inject(HttpWrapperService);
  private refreshTrigger = new BehaviorSubject<void>(undefined);

  getAll(): Observable<CheckingAccountResponse[]> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<CheckingAccountResponse[]>('GET', this.BASE_URL)
      )
    );
  }

  getById(id: string): Observable<CheckingAccountResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<CheckingAccountResponse>(
          'GET',
          `${this.BASE_URL}/${id}`
        )
      )
    );
  }

  create(
    checkingAccount: CheckingAccountRequest
  ): Observable<CheckingAccountResponse> {
    return this.http
      .handleRequest<CheckingAccountResponse>('POST', this.BASE_URL, {
        body: checkingAccount,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  update(id: string, changes: Partial<CheckingAccountRequest>) {
    return this.http
      .handleRequest('PUT', `${this.BASE_URL}/${id}`, {
        body: changes,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  delete(id: string) {
    return this.http
      .handleRequest('DELETE', `${this.BASE_URL}/${id}`)
      .pipe(tap(() => this.refreshTrigger.next()));
  }
}
