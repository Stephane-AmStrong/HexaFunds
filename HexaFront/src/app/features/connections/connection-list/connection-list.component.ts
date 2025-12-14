import { ChangeDetectionStrategy, Component, computed, inject, model } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { ConnectionResponse } from '../../../core/models/responses/connection-response';
import { ActivatedRoute, Router } from '@angular/router';
import { ToolbarComponent } from '../../../shared/components/toolbar/toolbar.component';

import { toSignal } from '@angular/core/rxjs-interop';
import { ConnectionQueryParameters } from '../../../core/models/query-parameters/connection-query-parameters';
import { MatCardModule } from '@angular/material/card';
import { ConnectionService } from '../services/connection.service';
import { MatTableModule } from '@angular/material/table';
import { ConnectionTableComponent } from '../connection-table/connection-table.component';

@Component({
  selector: 'connection-list',
  imports: [MatCardModule, MatGridListModule, MatTableModule, MatButtonModule, ToolbarComponent, ConnectionTableComponent],
  templateUrl: './connection-list.component.html',
  styleUrl: './connection-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConnectionListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly connectionsService = inject(ConnectionService);
  
  private dialog = inject(MatDialog);

  private readonly queryParams = computed<ConnectionQueryParameters>(() => ({
    withName: this.route.snapshot.params['withName'],
    withAppName: this.route.snapshot.params['withAppName'],
    withVersion: this.route.snapshot.params['withVersion'],
    searchTerm: this.route.snapshot.queryParams['searchTerm'],
    orderBy: this.route.snapshot.queryParams['orderBy'],
    page: +this.route.snapshot.queryParams['page'] || 1,
    pageSize: +this.route.snapshot.queryParams['pageSize'] || 10,
  }));

  readonly connections = toSignal<ConnectionResponse[], ConnectionResponse[]>(
    this.connectionsService.getPagedListByQueryAsync(this.queryParams()),
    { initialValue: [] }
  );
}
