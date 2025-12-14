import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'error-message',
  imports: [RouterLink, MatButtonModule],
  templateUrl: './error-message.component.html',
  styleUrl: './error-message.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ErrorMessageComponent {
  imageUrl = input<string>();
  title = input<string>();
  message = input<string>();
  actionLabel = input<string | undefined>();
  actionUrl = input<string | undefined>();
}
