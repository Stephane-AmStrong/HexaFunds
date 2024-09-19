import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { CheckingAccountResponse } from '../models/checking-account-response';
import { CheckingAccountRequest } from '../models/checking-account-request';

@Injectable({
  providedIn: 'root',
})
export class CheckingAccountService {
  private BASE_URL: string = `checkingaccounts`;
  private http = inject(HttpClient);

  getAll(): Observable<CheckingAccountResponse[]> {
    return this.http
      .get<CheckingAccountResponse[]>(this.BASE_URL)
      .pipe(shareReplay());
  }

  getById(id: string): Observable<CheckingAccountResponse> {
    return this.http
      .get<CheckingAccountResponse>(`${this.BASE_URL}/${id}`)
      .pipe(shareReplay());
  }

  create(
    checkingAccount: CheckingAccountRequest
  ): Observable<CheckingAccountResponse> {
    return this.http
      .post<CheckingAccountResponse>(this.BASE_URL, checkingAccount)
      .pipe(shareReplay());
  }

  update(id: string, changes: Partial<CheckingAccountRequest>) {
    return this.http
      .put<CheckingAccountResponse>(`${this.BASE_URL}/${id}`, changes)
      .pipe(shareReplay());
  }

  delete(id: string) {
    return this.http.delete(`${this.BASE_URL}/${id}`).pipe(shareReplay());
  }
}
