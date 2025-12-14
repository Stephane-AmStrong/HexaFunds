import { ChangeDetectionStrategy, Component, computed, inject, model } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { ServerResponse } from '../../../core/models/responses/server-response';
import { ActivatedRoute, Router } from '@angular/router';
import { ToolbarComponent } from '../../../shared/components/toolbar/toolbar.component';

import { toSignal } from '@angular/core/rxjs-interop';
import { ServerQueryParameters } from '../../../core/models/query-parameters/server-query-parameters';
import { MatCardModule } from '@angular/material/card';
import { ServerService } from '../services/server.service';
import { ServerTableComponent } from '../server-table/server-table.component';

@Component({
  selector: 'server-list',
  imports: [MatCardModule, MatGridListModule, MatButtonModule, ToolbarComponent, ServerTableComponent],
  templateUrl: './server-list.component.html',
  styleUrl: './server-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ServerListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly serversService = inject(ServerService);

  private dialog = inject(MatDialog);

  private readonly queryParams = computed<ServerQueryParameters>(() => ({
    withName: this.route.snapshot.params['withName'],
    withAppName: this.route.snapshot.params['withAppName'],
    withVersion: this.route.snapshot.params['withVersion'],
    searchTerm: this.route.snapshot.queryParams['searchTerm'],
    orderBy: this.route.snapshot.queryParams['orderBy'],
    page: +this.route.snapshot.queryParams['page'] || 1,
    pageSize: +this.route.snapshot.queryParams['pageSize'] || 10,
  }));

  readonly servers = toSignal<ServerResponse[], ServerResponse[]>(
    this.serversService.getPagedListByQueryAsync(this.queryParams()),
    { initialValue: [] }
  );
}
