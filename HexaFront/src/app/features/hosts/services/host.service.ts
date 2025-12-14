import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, startWith, switchMap, tap } from 'rxjs';
import { HostResponse } from '../../../core/models/responses/host-response';
import { BaseApiService } from '../../../core/services/base-api.service';
import { HostCreateRequest } from '../../../core/models/requests/host-create-request';
import { API_ENDPOINTS } from '../../../core/constants/api-endpoints';
import { HostDetailedResponse } from '../../../core/models/responses/host-detailed-response';
import { HostQueryParameters } from '../../../core/models/query-parameters/HostQueryParameters';
import { ServerDetailedResponse } from '../../../core/models/responses/server-detailed-response';

@Injectable({
  providedIn: 'root',
})
export class HostService {
  private http = inject(BaseApiService);
  private refreshTrigger = new BehaviorSubject<void>(undefined);

  getPagedListByQueryAsync(queryParams: HostQueryParameters): Observable<HostDetailedResponse[]> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<HostDetailedResponse[]>('GET', API_ENDPOINTS.HOSTS.WITH_SERVERS(), { params: queryParams })
      )
    );
  }

  getByName(name: string): Observable<HostDetailedResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<HostDetailedResponse>(
          'GET',API_ENDPOINTS.HOSTS.BY_NAME(name)
        )
      )
    );
  }

  getServerByHostAndApp(hostName: string, appName: string): Observable<ServerDetailedResponse> {
    return this.refreshTrigger.pipe(
      startWith(undefined),
      switchMap(() =>
        this.http.handleRequest<ServerDetailedResponse>(
          'GET', API_ENDPOINTS.HOSTS.SERVER_APP(hostName, appName)
        )
      )
    );
  }

  create(
    host: HostCreateRequest
  ): Observable<HostResponse> {
    return this.http
      .handleRequest<HostResponse>('POST', API_ENDPOINTS.HOSTS.ROOT, {
        body: host,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  update(id: string, changes: Partial<HostCreateRequest>) {
    return this.http
      .handleRequest('PUT', API_ENDPOINTS.HOSTS.BY_ID(id), {
        body: changes,
      })
      .pipe(tap(() => this.refreshTrigger.next()));
  }

  delete(id: string) {
    return this.http
      .handleRequest('DELETE', API_ENDPOINTS.HOSTS.BY_ID(id))
      .pipe(tap(() => this.refreshTrigger.next()));
  }

}
