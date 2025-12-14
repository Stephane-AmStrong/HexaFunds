import { Component, inject, input } from '@angular/core';
import { ServerResponse } from '../../../core/models/responses/server-response';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { PrefixPictogramComponent } from '../../../shared/components/prefix-pictogram/prefix-pictogram.component';
import { ServerStatusIndicatorComponent } from '../server-status-indicator/server-status-indicator.component';
import { CronDescriptionPipe } from "../../../shared/pipes/cron-description.pipe";
import { HashColorPipe } from "../../../shared/pipes/hash-color.pipe";

@Component({
  selector: 'server-table',
  imports: [MatTableModule, PrefixPictogramComponent, ServerStatusIndicatorComponent, CronDescriptionPipe, HashColorPipe],
  templateUrl: './server-table.component.html',
  styleUrl: './server-table.component.scss',
})
export class ServerTableComponent {
  private readonly router = inject(Router);

  servers = input.required<ServerResponse[]>();
  serverColumns = input<string[]>([
    'hostName',
    'appName',
    'version',
    'port',
    'type',
    'cronStartTime',
    'cronStopTime',
  ])

  onServerClicked(server: ServerResponse) {
    const hostName = server.hostName;
    const appName = server.appName;
    if (hostName && appName) {
      this.router.navigate(['/hosts', hostName, 'servers', appName]);
    }
  }
}
