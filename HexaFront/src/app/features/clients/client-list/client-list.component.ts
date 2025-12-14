import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { ClientResponse } from '../../../core/models/responses/client-response';
import { ActivatedRoute } from '@angular/router';
import { ToolbarComponent } from '../../../shared/components/toolbar/toolbar.component';

import { toSignal } from '@angular/core/rxjs-interop';
import { ClientQueryParameters } from '../../../core/models/query-parameters/client-query-parameters';
import { MatCardModule } from '@angular/material/card';
import { ClientService } from '../services/client.service';
import { ClientTableComponent } from '../client-table/client-table.component';

@Component({
  selector: 'client-list',
  imports: [MatCardModule, MatGridListModule, MatButtonModule, ToolbarComponent, ClientTableComponent],
  templateUrl: './client-list.component.html',
  styleUrl: './client-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClientListComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly clientsService = inject(ClientService);

  private dialog = inject(MatDialog);

  private readonly queryParams = computed<ClientQueryParameters>(() => ({
    withGaia: this.route.snapshot.params['withGaia'],
    withLogin: this.route.snapshot.params['withLogin'],
    searchTerm: this.route.snapshot.queryParams['searchTerm'],
    orderBy: this.route.snapshot.queryParams['orderBy'],
    page: +this.route.snapshot.queryParams['page'] || 1,
    pageSize: +this.route.snapshot.queryParams['pageSize'] || 10,
  }));

  readonly clients = toSignal<ClientResponse[], ClientResponse[]>(
    this.clientsService.getPagedListByQueryAsync(this.queryParams()),
    { initialValue: [] }
  );
}
