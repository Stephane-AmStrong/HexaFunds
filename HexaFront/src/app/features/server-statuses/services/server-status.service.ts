import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, startWith, switchMap, tap } from 'rxjs';
import { BaseApiService } from '../../../core/services/base-api.service';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints';
import { ServerStatusQueryParameters } from '../../../core/models/query-parameters/server-status-query-parameters';
import { ServerStatusCreateRequest } from '../../../core/models/requests/server-status-create-request';
import { ServerStatusDetailedResponse } from '../../../core/models/responses/server-status-detailed-response';
import { ServerStatusResponse } from '../../../core/models/responses/server-status-response';

@Injectable({
  providedIn: 'root',
})
export class ServerStatusService {
  private http = inject(BaseApiService);
  private refreshTrigger = new BehaviorSubject<void>(undefined);

  getPagedListByQueryAsync(queryParams: ServerStatusQueryParameters): Observable<ServerStatusResponse[]> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ServerStatusResponse[]>('GET', API_ENDPOINTS.SERVER_STATUSES.ROOT, { params: queryParams })
      )
    );
  }

  getById(id: string): Observable<ServerStatusDetailedResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ServerStatusDetailedResponse>(
          'GET', API_ENDPOINTS.SERVER_STATUSES.BY_ID(id)
        )
      )
    );
  }

  create(
    serverstatus: ServerStatusCreateRequest
  ): Observable<ServerStatusResponse> {
    return this.http
      .handleRequest<ServerStatusResponse>('POST', API_ENDPOINTS.SERVER_STATUSES.ROOT, {
        body: serverstatus,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  update(id: string, changes: Partial<ServerStatusCreateRequest>) {
    return this.http
      .handleRequest('PUT', API_ENDPOINTS.SERVER_STATUSES.BY_ID(id), {
        body: changes,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  delete(id: string) {
    return this.http
      .handleRequest('DELETE', API_ENDPOINTS.SERVER_STATUSES.BY_ID(id))
      .pipe(tap(() => this.refreshTrigger.next()));
  }

}
