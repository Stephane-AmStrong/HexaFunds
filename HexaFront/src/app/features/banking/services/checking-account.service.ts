import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { CheckingAccountResponse } from '../models/checking-account-response';

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
}
