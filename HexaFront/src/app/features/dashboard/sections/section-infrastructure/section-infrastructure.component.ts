import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { ChartConfig } from '../../chart-config';

@Component({
  selector: 'section-infrastructure',
  imports: [MatGridListModule, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './section-infrastructure.component.html',
  styleUrl: './section-infrastructure.component.scss',
})
export class SectionInfrastructureComponent {
  cards: ChartConfig[] = [
    {
      title: 'Card 27',
      cols: 4,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 28',
      cols: 4,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 29',
      cols: 4,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 30',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 31',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 32',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 33',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 34',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 35',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 36',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 37',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
  ];
}
