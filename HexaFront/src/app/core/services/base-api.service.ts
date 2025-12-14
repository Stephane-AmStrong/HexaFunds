import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { throwError, Observable, shareReplay, catchError } from 'rxjs';
import { HttpMethod } from './http-method';
import { RequestOptions } from './request-options';

@Injectable({
  providedIn: 'root'
})
export class BaseApiService {
    private handleError(error: Error) {
    console.error('Error occurred:', error);
    return throwError(() => error);
  }

  private http = inject(HttpClient);

   private toHttpParams(params?: Record<string, string | number | boolean | undefined | null>): HttpParams | undefined {
    if (!params) return undefined;

    const cleanParams: Record<string, string> = {};

    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null && value !== '') {
        cleanParams[key] = String(value);
      }
    });

    if (Object.keys(cleanParams).length === 0) return undefined;

    return new HttpParams({ fromObject: cleanParams });
  }

  private httpMethods: Record<HttpMethod,<T>(url: string, options?: RequestOptions<T>) => Observable<any>
  > = {
    GET: (url, options?) => this.http.get(url, {
      headers: options?.headers,
      params: this.toHttpParams(options?.params)
    }),
    POST: (url, options?) => this.http.post(url, options?.body, {
      headers: options?.headers,
      params: this.toHttpParams(options?.params)
    }),
    PUT: (url, options?) => this.http.put(url, options?.body, {
      headers: options?.headers,
      params: this.toHttpParams(options?.params)
    }),
    DELETE: (url, options?) => this.http.delete(url, {
      headers: options?.headers,
      params: this.toHttpParams(options?.params)
    }),
  };

  handleRequest<T, U = unknown>(
    method: HttpMethod,
    url: string,
    options?: RequestOptions<U>
  ): Observable<T> {
    return this.httpMethods[method](url, options).pipe(
      shareReplay(),
      catchError(this.handleError)
    );
  }
}
