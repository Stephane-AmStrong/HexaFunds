import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { AlertResponse } from '../../../core/models/responses/alert-response';
import { BaseApiService } from '../../../core/services/base-api.service';
import { AlertCreateOrIncrementRequest } from '../../../core/models/requests/alert-create-or-increment-request';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints';
import { AlertQueryParameters } from '../../../core/models/query-parameters/alert-query-parameters';
import { AlertDetailedResponse } from '../../../core/models/responses/alert-detailed-response';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private http = inject(BaseApiService);
  private refreshTrigger = new BehaviorSubject<void>(undefined);

  getPagedListByQueryAsync(queryParams: AlertQueryParameters): Observable<AlertResponse[]> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<AlertResponse[]>('GET', API_ENDPOINTS.ALERTS.ROOT, { params: queryParams })
      )
    );
  }

  getById(id: string): Observable<AlertDetailedResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<AlertDetailedResponse>(
          'GET', API_ENDPOINTS.ALERTS.BY_ID(id)
        )
      )
    );
  }

  create(
    alert: AlertCreateOrIncrementRequest
  ): Observable<AlertResponse> {
    return this.http
      .handleRequest<AlertResponse>('POST', API_ENDPOINTS.ALERTS.ROOT, {
        body: alert,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  update(id: string, changes: Partial<AlertCreateOrIncrementRequest>) {
    return this.http
      .handleRequest('PUT', API_ENDPOINTS.ALERTS.BY_ID(id), {
        body: changes,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  delete(id: string) {
    return this.http
      .handleRequest('DELETE', API_ENDPOINTS.ALERTS.BY_ID(id))
      .pipe(tap(() => this.refreshTrigger.next()));
  }

}
