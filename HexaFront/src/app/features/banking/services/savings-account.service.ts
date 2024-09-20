import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { SavingsAccountRequest } from '../models/savings-account-request';
import { SavingsAccountResponse } from '../models/savings-account-response';

@Injectable({
  providedIn: 'root'
})
export class SavingsAccountService {

  private BASE_URL: string = `savingsAccounts`;
  private http = inject(HttpClient);

  getAll(): Observable<SavingsAccountResponse[]> {
    return this.http
      .get<SavingsAccountResponse[]>(this.BASE_URL)
      .pipe(shareReplay());
  }

  getById(id: string): Observable<SavingsAccountResponse> {
    return this.http
      .get<SavingsAccountResponse>(`${this.BASE_URL}/${id}`)
      .pipe(shareReplay());
  }

  create(
    savingsAccount: SavingsAccountRequest
  ): Observable<SavingsAccountResponse> {
    return this.http
      .post<SavingsAccountResponse>(this.BASE_URL, savingsAccount)
      .pipe(shareReplay());
  }

  update(id: string, changes: Partial<SavingsAccountRequest>) {
    return this.http
      .put<SavingsAccountResponse>(`${this.BASE_URL}/${id}`, changes)
      .pipe(shareReplay());
  }

  delete(id: string) {
    return this.http.delete(`${this.BASE_URL}/${id}`).pipe(shareReplay());
  }
}
