import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatCardModule } from "@angular/material/card";
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ClientResponse } from '../../../core/models/responses/client-response';
import { MatListModule } from "@angular/material/list";
import { MatExpansionModule } from '@angular/material/expansion';
import { MatRippleModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'client-card',
  imports: [MatCardModule, MatChipsModule, MatProgressBarModule, MatListModule, MatExpansionModule, MatButtonModule, MatRippleModule],
  templateUrl: './client-card.component.html',
  styleUrl: './client-card.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClientCardComponent {
  client = input.required<ClientResponse>();

  onCopyToClipboard(event: Event) {
    event.stopPropagation();
    console.log('Copy to clipboard not implemented yet');
  }

  onShare(event: Event) {
    event.stopPropagation();
    console.log('Share not implemented yet');
  }

}
