import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { ConnectionResponse } from '../../../core/models/responses/connection-response';
import { MatTableModule } from '@angular/material/table';
import { PrefixPictogramComponent } from "../../../shared/components/prefix-pictogram/prefix-pictogram.component";
import { HashColorPipe } from "../../../shared/pipes/hash-color.pipe";

@Component({
  selector: 'connection-table',
  imports: [MatTableModule, PrefixPictogramComponent, HashColorPipe],
  templateUrl: './connection-table.component.html',
  styleUrl: './connection-table.component.scss'
})
export class ConnectionTableComponent {
  private readonly router = inject(Router);

  connections = input.required<ConnectionResponse[]>();

  readonly connectionColumns: readonly string[] = ['clientId', 'serverId', 'application', 'apiVersion', 'machine', 'processId'] as const;

  onConnectionClicked(connection: ConnectionResponse) {
    const connectionId = connection.id;
    if (connectionId) {
      this.router.navigate(['/connections', connectionId]);
    }
  }
}
