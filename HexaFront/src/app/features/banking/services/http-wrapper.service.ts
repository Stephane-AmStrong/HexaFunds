import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError, timer } from 'rxjs';
import { catchError, retry, shareReplay } from 'rxjs/operators';

export type HttpMethod = 'GET' | 'POST' | 'PUT' | 'DELETE';

export interface RequestOptions<T> {
  body?: T;
  params?: HttpParams;
  headers?: HttpHeaders;
}

interface RetryConfig {
  count: number;
  delay: (error: Error, retryCount: number) => Observable<number>;
}

@Injectable({
  providedIn: 'root',
})
export class HttpWrapperService {
  private readonly RETRY_COUNT = 3;

  private retryStrategy: RetryConfig = {
    count: this.RETRY_COUNT,
    delay: (error: Error, retryCount: number) => timer(2 ** retryCount * 1000),
  };

  private handleError(error: Error) {
    console.error('Error occurred:', error);
    return throwError(() => error);
  }

  private http = inject(HttpClient);

  private httpMethods: Record<
    HttpMethod,
    <T>(url: string, options?: RequestOptions<T>) => Observable<any>
  > = {
    GET: (url, options?) => this.http.get(url, { ...options }),
    POST: (url, options?) => this.http.post(url, options?.body, { ...options }),
    PUT: (url, options?) => this.http.put(url, options?.body, { ...options }),
    DELETE: (url, options?) => this.http.delete(url, { ...options }),
  };

  handleRequest<T, U = unknown>(
    method: HttpMethod,
    url: string,
    options?: RequestOptions<U>
  ): Observable<T> {
    return this.httpMethods[method](url, options).pipe(
      retry({
        count: this.retryStrategy.count,
        delay: this.retryStrategy.delay,
      }),
      shareReplay(),
      catchError(this.handleError)
    );
  }
}
