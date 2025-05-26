import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SavingsAccountRequest } from '../models/savings-account-request';
import { SavingsAccountResponse } from '../models/savings-account-response';
import { HttpWrapperService } from './http-wrapper.service';

@Injectable({
  providedIn: 'root',
})
export class SavingsAccountService {
  private BASE_URL: string = `savingsAccounts`;
  private http = inject(HttpWrapperService);

  getAll(): Observable<SavingsAccountResponse[]> {
    return this.http.handleRequest('GET', this.BASE_URL);
  }

  getById(id: string): Observable<SavingsAccountResponse> {
    return this.http.handleRequest('GET', `${this.BASE_URL}/${id}`);
  }

  create(
    savingsAccount: SavingsAccountRequest
  ): Observable<SavingsAccountResponse> {
    return this.http.handleRequest('POST', this.BASE_URL, {
      body: savingsAccount,
    });
  }

  update(id: string, changes: Partial<SavingsAccountRequest>) {
    return this.http.handleRequest('PUT', `${this.BASE_URL}/${id}`, {
      body: changes,
    });
  }

  delete(id: string) {
    return this.http.handleRequest('DELETE', `${this.BASE_URL}/${id}`);
  }
}
