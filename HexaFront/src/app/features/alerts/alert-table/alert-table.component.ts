import { Component, inject, input } from '@angular/core';
import { AlertResponse } from '../../../core/models/responses/alert-response';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'alert-table',
  imports: [MatTableModule],
  templateUrl: './alert-table.component.html',
  styleUrl: './alert-table.component.scss'
})
export class AlertTableComponent {
  private readonly router = inject(Router);
  alerts = input.required<AlertResponse[]>();
  
  readonly alertColumns: readonly string[] = ['type', 'message', 'severity', 'occurrence', 'status', 'occurredAt'] as const;
  
  onAlertClicked(alert: AlertResponse) {
    const alertId = alert.id;
    if (alertId) {
      this.router.navigate(['/alerts', alertId]);
    }
  }
}
