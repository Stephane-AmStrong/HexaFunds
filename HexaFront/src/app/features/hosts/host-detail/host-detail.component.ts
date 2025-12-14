import { ChangeDetectionStrategy, Component, computed, inject, model } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatGridListModule } from '@angular/material/grid-list';
import { ActivatedRoute, Router } from '@angular/router';
import { ToolbarComponent } from '../../../shared/components/toolbar/toolbar.component';

import { HostDetailedResponse } from '../../../core/models/responses/host-detailed-response';
import { ServerResponse } from '../../../core/models/responses/server-response';
import { MatTableModule } from '@angular/material/table';
import { toSignal } from '@angular/core/rxjs-interop';
import { HostService } from '../services/host.service';
import { MatCardModule } from '@angular/material/card';
import { MatIcon } from '@angular/material/icon';
import { ServerTableComponent } from "../../servers/server-table/server-table.component";

@Component({
  selector: 'host-detail',
  imports: [MatCardModule, MatGridListModule, MatIcon, MatTableModule, MatButtonModule, ToolbarComponent, ServerTableComponent],
  templateUrl: './host-detail.component.html',
  styleUrl: './host-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HostDetailComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly hostsService = inject(HostService);

  readonly hostName = computed<string>(() => this.route.snapshot.params['hostName']);

  readonly hostDetail = toSignal<HostDetailedResponse>(
    this.hostsService.getByName(this.hostName()),
    { initialValue: null }
  );

  readonly servers = computed<ServerResponse[]>(() => this.hostDetail()?.servers || []);

  private dialog = inject(MatDialog);

  serverColumns: string[] = ['appName', 'version', 'port', 'type', 'cronStartTime', 'cronStopTime'] as const;

  onRowClicked(row: ServerResponse) {
    const hostName = row.hostName;
    const appName = row.appName;
    if (hostName && appName) {
      this.router.navigate(['/hosts', hostName, 'servers', appName]);
    }
  }
}
