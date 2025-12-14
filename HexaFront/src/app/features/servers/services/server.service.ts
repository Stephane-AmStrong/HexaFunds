import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { ServerResponse } from '../../../core/models/responses/server-response';
import { BaseApiService } from '../../../core/services/base-api.service';
import { ServerCreateRequest } from '../../../core/models/requests/server-create-request';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints';
import { ServerQueryParameters } from '../../../core/models/query-parameters/server-query-parameters';
import { ServerDetailedResponse } from '../../../core/models/responses/server-detailed-response';

@Injectable({
  providedIn: 'root',
})
export class ServerService {
  private http = inject(BaseApiService);
  private refreshTrigger = new BehaviorSubject<void>(undefined);

  getPagedListByQueryAsync(queryParams: ServerQueryParameters): Observable<ServerResponse[]> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ServerResponse[]>('GET', API_ENDPOINTS.SERVERS.ROOT, { params: queryParams })
      )
    );
  }

  getById(id: string): Observable<ServerDetailedResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ServerDetailedResponse>(
          'GET', API_ENDPOINTS.SERVERS.BY_ID(id)
        )
      )
    );
  }

  create(
    server: ServerCreateRequest
  ): Observable<ServerResponse> {
    return this.http
      .handleRequest<ServerResponse>('POST', API_ENDPOINTS.SERVERS.ROOT, {
        body: server,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  update(id: string, changes: Partial<ServerCreateRequest>) {
    return this.http
      .handleRequest('PUT', API_ENDPOINTS.SERVERS.BY_ID(id), {
        body: changes,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  delete(id: string) {
    return this.http
      .handleRequest('DELETE', API_ENDPOINTS.SERVERS.BY_ID(id))
      .pipe(tap(() => this.refreshTrigger.next()));
  }

}
