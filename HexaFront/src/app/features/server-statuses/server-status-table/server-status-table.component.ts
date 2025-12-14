import { Component, inject, input } from '@angular/core';
import { ServerStatusResponse } from '../../../core/models/responses/server-status-response';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'server-status-table',
  imports: [MatTableModule],
  templateUrl: './server-status-table.component.html',
  styleUrl: './server-status-table.component.scss'
})
export class ServerStatusTableComponent {
  private readonly router = inject(Router);
  serverStatuses = input.required<ServerStatusResponse[]>();

  readonly serverStatusColumns: readonly string[] = ['status', 'recordedAt'] as const;

  onServerStatusClicked(serverStatus: ServerStatusResponse) {
    const serverStatusId = serverStatus.id;
    if (serverStatusId) {
      this.router.navigate(['/server-statuses', serverStatusId]);
    }
  }
}
