import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { AlertResponse } from '../../../core/models/responses/alert-response';
import { ActivatedRoute } from '@angular/router';
import { ToolbarComponent } from '../../../shared/components/toolbar/toolbar.component';

import { toSignal } from '@angular/core/rxjs-interop';
import { AlertQueryParameters } from '../../../core/models/query-parameters/alert-query-parameters';
import { MatCardModule } from '@angular/material/card';
import { AlertService } from '../services/alert.service';
import { MatTableModule } from '@angular/material/table';
import { AlertTableComponent } from '../alert-table/alert-table.component';

@Component({
  selector: 'alert-list',
  imports: [MatCardModule, MatGridListModule, MatTableModule, MatButtonModule, ToolbarComponent, AlertTableComponent],
  templateUrl: './alert-list.component.html',
  styleUrl: './alert-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AlertListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly alertsService = inject(AlertService);

  private dialog = inject(MatDialog);

  private readonly queryParams = computed<AlertQueryParameters>(() => ({
    withName: this.route.snapshot.params['withName'],
    withAppName: this.route.snapshot.params['withAppName'],
    withVersion: this.route.snapshot.params['withVersion'],
    searchTerm: this.route.snapshot.queryParams['searchTerm'],
    orderBy: this.route.snapshot.queryParams['orderBy'],
    page: +this.route.snapshot.queryParams['page'] || 1,
    pageSize: +this.route.snapshot.queryParams['pageSize'] || 10,
  }));

  readonly alerts = toSignal<AlertResponse[], AlertResponse[]>(
    this.alertsService.getPagedListByQueryAsync(this.queryParams()),
    { initialValue: [] }
  );
}
