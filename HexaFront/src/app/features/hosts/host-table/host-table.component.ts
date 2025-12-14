import { Component, computed, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { HostDetailedResponse } from '../../../core/models/responses/host-detailed-response';
import { MatTableModule } from '@angular/material/table';
import { ServerStatusIndicatorComponent } from '../../servers/server-status-indicator/server-status-indicator.component';
import { PrefixPictogramComponent } from "../../../shared/components/prefix-pictogram/prefix-pictogram.component";
import { HostServerRow } from './host-server-row';
import { CronDescriptionPipe } from '../../../shared/pipes/cron-description.pipe';
import { HashColorPipe } from "../../../shared/pipes/hash-color.pipe";

@Component({
  selector: 'host-table',
  imports: [MatTableModule, ServerStatusIndicatorComponent, PrefixPictogramComponent, CronDescriptionPipe, HashColorPipe],
  templateUrl: './host-table.component.html',
  styleUrl: './host-table.component.scss',
})
export class HostTableComponent {
  private readonly router = inject(Router);

  hosts = input.required<HostDetailedResponse[]>();
  readonly dataSource = computed<HostServerRow[]>(() => this.flattenHosts(this.hosts()));

  readonly serverColumns: readonly string[] = [
    'hostName',
    'appName',
    'version',
    'port',
    'type',
    'cronStartTime',
    'cronStopTime',
  ] as const;

  onHostClicked(row: HostServerRow, event: MouseEvent): void {
    event.stopPropagation();
    const hostName = row.host.name;
    if (hostName) {
      this.router.navigate(['/hosts', hostName]);
    }
  }

  onServerClicked(row: HostServerRow): void {
    const hostName = row.host.name;
    const appName = row.server.appName;
    if (hostName && appName) {
      this.router.navigate(['/hosts', hostName, 'servers', appName]);
    }
  }

  private flattenHosts(hosts: HostDetailedResponse[]): HostServerRow[] {
    return hosts.flatMap((host) =>
      host.servers.map((server, index) => ({
        host,
        server,
        hostRowSpan: host.servers.length,
        isFirstServer: index === 0,
      }))
    );
  }
}