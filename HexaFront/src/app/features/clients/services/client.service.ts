import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { ClientResponse } from '../../../core/models/responses/client-response';
import { BaseApiService } from '../../../core/services/base-api.service';
import { ClientCreateRequest } from '../../../core/models/requests/client-create-request';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints';
import { ClientQueryParameters } from '../../../core/models/query-parameters/client-query-parameters';
import { ClientDetailedResponse } from '../../../core/models/responses/client-detailed-response';
import { MOCK_CLIENTS } from '../MOCK_CLIENTS';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private http = inject(BaseApiService);
  private refreshTrigger = new BehaviorSubject<void>(undefined);

  getPagedListByQueryAsync(queryParams: ClientQueryParameters): Observable<ClientResponse[]> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ClientResponse[]>('GET', API_ENDPOINTS.CLIENTS.ROOT, { params: queryParams })
      )
    );
  }

  getById(id: string): Observable<ClientDetailedResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ClientDetailedResponse>(
          'GET', API_ENDPOINTS.CLIENTS.BY_ID(id)
        )
      )
    );
  }

  create(
    client: ClientCreateRequest
  ): Observable<ClientResponse> {
    return this.http
      .handleRequest<ClientResponse>('POST', API_ENDPOINTS.CLIENTS.ROOT, {
        body: client,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  update(id: string, changes: Partial<ClientCreateRequest>) {
    return this.http
      .handleRequest('PUT', API_ENDPOINTS.CLIENTS.BY_ID(id), {
        body: changes,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  delete(id: string) {
    return this.http
      .handleRequest('DELETE', API_ENDPOINTS.CLIENTS.BY_ID(id))
      .pipe(tap(() => this.refreshTrigger.next()));
  }

}
