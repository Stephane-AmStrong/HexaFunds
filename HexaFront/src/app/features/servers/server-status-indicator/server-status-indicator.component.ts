import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'server-status-indicator',
  imports: [MatIconModule, MatListModule],
  templateUrl: './server-status-indicator.component.html',
  styleUrl: './server-status-indicator.component.scss',
})
export class ServerStatusIndicatorComponent {
  status = input.required<'Up' | 'Down' | undefined>();
}
