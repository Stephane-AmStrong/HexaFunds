import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatCardModule } from "@angular/material/card";
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ServerResponse } from '../../../core/models/responses/server-response';
import { MatIcon } from '@angular/material/icon';
import { MatListModule } from "@angular/material/list";
import { MatExpansionModule } from '@angular/material/expansion';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'server-card',
  imports: [MatCardModule, MatChipsModule, MatProgressBarModule, MatIcon, MatListModule, MatExpansionModule, MatButtonModule, MatRippleModule],
  templateUrl: './server-card.component.html',
  styleUrl: './server-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServerCardComponent {
  server = input.required<ServerResponse>();

  onCopyToClipboard(event: Event) {
    event.stopPropagation();
    console.log('Copy to clipboard not implemented yet');
  }

  onShare(event: Event) {
    event.stopPropagation();
    console.log('Share not implemented yet');
  }

}
