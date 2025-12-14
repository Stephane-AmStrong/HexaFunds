import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatListModule } from '@angular/material/list';
import { ToolbarComponent } from "../../../shared/components/toolbar/toolbar.component";
import { ClientDetailedResponse } from '../../../core/models/responses/client-detailed-response';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../../clients/services/client.service';
import { ConnectionTableComponent } from '../../connections/connection-table/connection-table.component';
import { ConnectionService } from '../../connections/services/connection.service';
import { ConnectionResponse } from '../../../core/models/responses/connection-response';
import { DetailCardComponent } from "../../../shared/components/detail-card/detail-card.component";

@Component({
  selector: 'client-detail',
  imports: [
    ToolbarComponent,
    MatTabsModule,
    MatCardModule,
    MatIconModule,
    MatGridListModule,
    MatDividerModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatListModule,
    ConnectionTableComponent,
    DetailCardComponent
],
  templateUrl: './client-detail.component.html',
  styleUrl: './client-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClientDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly connectionService = inject(ConnectionService);
  private readonly clientService = inject(ClientService);

  readonly id = computed<string>(() => this.route.snapshot.params['id']);

  readonly clientDetail = toSignal<ClientDetailedResponse>(
    this.clientService.getById(this.id()),
    { initialValue: null }
  );

  readonly connections = computed<ConnectionResponse[]>(() => this.clientDetail()?.connections || []);
}
