import { Component } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { ChartConfig } from '../../chart-config';

@Component({
  selector: 'section-monitoring-overview',
  imports: [MatGridListModule, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './section-monitoring-overview.component.html',
  styleUrl: './section-monitoring-overview.component.scss',
})
export class SectionMonitoringOverviewComponent {
  cards: ChartConfig[] = [
    {
      title: 'Card 1',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 2',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 3',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 4',
      cols: 1,
      rows: 1,
      seriesData: [{ data: [1, 2, 3] }],
      contentType: 'line-chart',
    },
  ];
}
