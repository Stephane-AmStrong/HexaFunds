import { Component, inject, input } from '@angular/core';
import { ClientResponse } from '../../../core/models/responses/client-response';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'client-table',
  imports: [MatTableModule],
  templateUrl: './client-table.component.html',
  styleUrl: './client-table.component.scss'
})
export class ClientTableComponent {
  private readonly router = inject(Router);
  clients = input.required<ClientResponse[]>();
  
  readonly clientColumns: readonly string[] = ['gaia', 'login'] as const;

  onClientClicked(client: ClientResponse) {
    const clientId = client.id;
    if (clientId) {
      this.router.navigate(['/clients', clientId]);
    }
  }

}
