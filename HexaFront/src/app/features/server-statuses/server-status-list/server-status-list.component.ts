import { ChangeDetectionStrategy, Component, computed, inject, model } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { ActivatedRoute, Router } from '@angular/router';
import { ToolbarComponent } from '../../../shared/components/toolbar/toolbar.component';

import { toSignal } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { ServerStatusQueryParameters } from '../../../core/models/query-parameters/server-status-query-parameters';
import { ServerStatusResponse } from '../../../core/models/responses/server-status-response';
import { ServerStatusService } from '../services/server-status.service';
import { ServerStatusTableComponent } from '../server-status-table/server-status-table.component';

@Component({
  selector: 'server-status-list',
  imports: [MatCardModule, MatGridListModule, MatTableModule, MatButtonModule, ToolbarComponent, ServerStatusTableComponent],
  templateUrl: './server-status-list.component.html',
  styleUrl: './server-status-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ServerStatusListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly serverstatusesService = inject(ServerStatusService);
  
  private dialog = inject(MatDialog);

  private readonly queryParams = computed<ServerStatusQueryParameters>(() => ({
    withName: this.route.snapshot.params['withName'],
    withAppName: this.route.snapshot.params['withAppName'],
    withVersion: this.route.snapshot.params['withVersion'],
    searchTerm: this.route.snapshot.queryParams['searchTerm'],
    orderBy: this.route.snapshot.queryParams['orderBy'],
    page: +this.route.snapshot.queryParams['page'] || 1,
    pageSize: +this.route.snapshot.queryParams['pageSize'] || 10,
  }));

  readonly serverStatuses = toSignal<ServerStatusResponse[], ServerStatusResponse[]>(
    this.serverstatusesService.getPagedListByQueryAsync(this.queryParams()),
    { initialValue: [] }
  );

  readonly serverStatusColumns: readonly string[] = ['hostName', 'appName', 'version', 'port', 'type', 'cronStartTime', 'cronStopTime'] as const;

  onServerStatusClicked(serverstatus: ServerStatusResponse) {
    const serverId = serverstatus.serverId;
    if (serverId) {
      this.router.navigate(['/servers', serverId, 'serverstatuses']);
    }
  }
}
