import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatDividerModule } from '@angular/material/divider';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatListModule } from '@angular/material/list';
import { DatePipe } from '@angular/common';
import { ToolbarComponent } from "../../../shared/components/toolbar/toolbar.component";
import { ServerDetailedResponse } from '../../../core/models/responses/server-detailed-response';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { ConnectionResponse } from '../../../core/models/responses/connection-response';
import { HostService } from '../../hosts/services/host.service';
import { ServerStatusResponse } from '../../../core/models/responses/server-status-response';
import { AlertTableComponent } from '../../alerts/alert-table/alert-table.component';
import { ConnectionTableComponent } from '../../connections/connection-table/connection-table.component';
import { ServerStatusTableComponent } from '../../server-statuses/server-status-table/server-status-table.component';
import { AlertService } from '../../alerts/services/alert.service';
import { AlertResponse } from '../../../core/models/responses/alert-response';
import { AlertQueryParameters } from '../../../core/models/query-parameters/alert-query-parameters';
import { DetailCardComponent } from "../../../shared/components/detail-card/detail-card.component";
import { ServerStatusIndicatorComponent } from "../server-status-indicator/server-status-indicator.component";
import { PrefixPictogramComponent } from "../../../shared/components/prefix-pictogram/prefix-pictogram.component";
import { CronDescriptionPipe } from "../../../shared/pipes/cron-description.pipe";
import { HashColorPipe } from "../../../shared/pipes/hash-color.pipe";

@Component({
  selector: 'server-detail',
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
    DatePipe,
    AlertTableComponent,
    ConnectionTableComponent,
    ServerStatusTableComponent,
    DetailCardComponent,
    ServerStatusIndicatorComponent,
    PrefixPictogramComponent,
    CronDescriptionPipe,
    HashColorPipe
],
  templateUrl: './server-detail.component.html',
  styleUrl: './server-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServerDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly alertService = inject(AlertService);
  private readonly hostService = inject(HostService);

  
  readonly hostName = computed<string>(() => this.route.snapshot.params['hostName']);
  readonly serverName = computed<string>(() => this.route.snapshot.params['serverName']);

  readonly serverDetail = toSignal<ServerDetailedResponse>(
    this.hostService.getServerByHostAndApp(this.hostName(), this.serverName()),
    { initialValue: null }
  );

  private readonly alertQueryParams = computed<AlertQueryParameters>(() => ({
    withServerId: this.serverDetail()?.id,
    orderBy: 'occurredAt desc',
  }));

  readonly alerts = toSignal<AlertResponse[], AlertResponse[]>(
    this.alertService.getPagedListByQueryAsync(this.alertQueryParams()),
    { initialValue: [] }
  );

  readonly connections = computed<ConnectionResponse[]>(() => this.serverDetail()?.connections || []);
  readonly statuses = computed<ServerStatusResponse[]>(() => this.serverDetail()?.statuses || []);
  readonly latestStatus = computed<ServerStatusResponse | null>(() => this.serverDetail()?.statuses[0] || null);
  readonly tags = computed<string[]>(() => this.serverDetail()?.tags || []);
}
