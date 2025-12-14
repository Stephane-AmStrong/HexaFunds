import { Component, input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'suffix-pictogram',
  imports: [MatIcon],
  templateUrl: './suffix-pictogram.component.html',
  styleUrl: './suffix-pictogram.component.scss',
})
export class SuffixPictogramComponent {
  icon = input.required<string>();
  label = input.required<string>();
  iconColor = input<string>();
}
