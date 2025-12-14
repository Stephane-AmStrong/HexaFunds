import { Component, input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'prefix-pictogram',
  imports: [MatIcon],
  templateUrl: './prefix-pictogram.component.html',
  styleUrl: './prefix-pictogram.component.scss',
})
export class PrefixPictogramComponent {
  icon = input.required<string>();
  label = input.required<string>();
  iconColor = input<string>();
}
