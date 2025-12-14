import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { ChartConfig } from '../../chart-config';

@Component({
  selector: 'section-database-network',
  imports: [MatGridListModule, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './section-database-network.component.html',
  styleUrl: './section-database-network.component.scss',
})
export class SectionDatabaseNetworkComponent {
  cards: ChartConfig[] = [
    {
      title: 'Card 38',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 39',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 40',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 41',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 42',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 43',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
  ];
}
