import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { NumericChartConfig } from '../../chart-config';

@Component({
  selector: 'section-applications',
  imports: [
    MatGridListModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule
],
  templateUrl: './section-applications.component.html',
  styleUrl: './section-applications.component.scss',
})
export class SectionApplicationsComponent {
  chartConfigs: NumericChartConfig[] = [
    {
      title: 'Card 16',
      cols: 4,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 17',
      cols: 4,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 18',
      cols: 4,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 19',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 20',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 21',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 22',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 23',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 24',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 25',
      cols: 7,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 26',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
  ];
}
