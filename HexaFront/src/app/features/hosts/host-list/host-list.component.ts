import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatTableModule } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';

import { ToolbarComponent } from '../../../shared/components/toolbar/toolbar.component';
import { HostService } from '../services/host.service';
import { HostDetailedResponse } from '../../../core/models/responses/host-detailed-response';
import { HostQueryParameters } from '../../../core/models/query-parameters/HostQueryParameters';
import { HostTableComponent } from '../host-table/host-table.component';
@Component({
  selector: 'host-list',
  imports: [MatCardModule, MatTableModule, MatGridListModule, MatButtonModule, ToolbarComponent, HostTableComponent],
  templateUrl: './host-list.component.html',
  styleUrl: './host-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HostListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly hostsService = inject(HostService);

  private readonly queryParams = computed<HostQueryParameters>(() => ({
    withName: this.route.snapshot.params['withName'],
    searchTerm: this.route.snapshot.queryParams['searchTerm'],
    orderBy: this.route.snapshot.queryParams['orderBy'],
    page: +this.route.snapshot.queryParams['page'] || 1,
    pageSize: +this.route.snapshot.queryParams['pageSize'] || 10,
  }));

  readonly hosts = toSignal<HostDetailedResponse[], HostDetailedResponse[]>(
    this.hostsService.getPagedListByQueryAsync(this.queryParams()),
    { initialValue: [] }
  );
}
