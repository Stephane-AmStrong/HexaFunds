import { Component, model } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'detail-card',
  imports: [MatButtonModule, MatCardModule, MatIconModule, MatMenuModule, MatGridListModule],
  templateUrl: './detail-card.component.html',
  styleUrl: './detail-card.component.scss'
})
export class DetailCardComponent {}