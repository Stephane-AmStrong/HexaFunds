import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { ConnectionResponse } from '../../../core/models/responses/connection-response';
import { BaseApiService } from '../../../core/services/base-api.service';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints';
import { ConnectionQueryParameters } from '../../../core/models/query-parameters/connection-query-parameters';
import { ConnectionDetailedResponse } from '../../../core/models/responses/connection-detailed-response';
import { ConnectionEstablishRequest } from '../../../core/models/requests/connection-establish-request';
import { MOCK_CONNECTIONS } from '../MOCK_CONNECTIONS';

@Injectable({
  providedIn: 'root',
})
export class ConnectionService {
  private http = inject(BaseApiService);
  private refreshTrigger = new BehaviorSubject<void>(undefined);

  getPagedListByQueryAsync(queryParams: ConnectionQueryParameters): Observable<ConnectionResponse[]> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ConnectionResponse[]>('GET', API_ENDPOINTS.CONNECTIONS.ROOT, { params: queryParams })
      )
    );
  }

  getById(id: string): Observable<ConnectionDetailedResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ConnectionDetailedResponse>(
          'GET', API_ENDPOINTS.CONNECTIONS.BY_ID(id)
        )
      )
    );
  }

  create(
    connection: ConnectionEstablishRequest
  ): Observable<ConnectionResponse> {
    return this.http
      .handleRequest<ConnectionResponse>('POST', API_ENDPOINTS.CONNECTIONS.ROOT, {
        body: connection,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  update(id: string, changes: Partial<ConnectionEstablishRequest>) {
    return this.http
      .handleRequest('PUT', API_ENDPOINTS.CONNECTIONS.BY_ID(id), {
        body: changes,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  delete(id: string) {
    return this.http
      .handleRequest('DELETE', API_ENDPOINTS.CONNECTIONS.BY_ID(id))
      .pipe(tap(() => this.refreshTrigger.next()));
  }

}
