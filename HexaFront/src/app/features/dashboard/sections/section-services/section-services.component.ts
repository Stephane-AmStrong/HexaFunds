import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { ChartConfig, DataPoint, HexagonChartConfig, NumericChartConfig } from '../../chart-config';
import { DashboardCardComponent } from "../../dashboard-card/dashboard-card.component";
import { AreaChartComponent } from "../../charts/area-chart/area-chart.component";
import { BarChartComponent } from "../../charts/bar-chart/bar-chart.component";
import { HoneycombChartComponent } from "../../charts/honeycomb-chart/honeycomb-chart.component";
import { LineChartComponent } from "../../charts/line-chart/line-chart.component";

@Component({
  selector: 'section-services',
  imports: [MatGridListModule, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule, DashboardCardComponent, AreaChartComponent, BarChartComponent, HoneycombChartComponent, LineChartComponent],
  templateUrl: './section-services.component.html',
  styleUrl: './section-services.component.scss',
})
export class SectionServicesComponent {
  honeycombData: DataPoint[] = [
    { name: 'Oregon', abbreviation: 'OR', x: 5, y: 5, value: 4233358 },
    { name: 'South Dakota', abbreviation: 'SD', x: 5, y: 6, value: 7812880 },
    { name: 'Vermont', abbreviation: 'VT', x: 5, y: 7, value: 647464 },
    { name: 'Washington', abbreviation: 'WA', x: 6, y: 8, value: 7812880 },

    { name: 'London', abbreviation: 'LN', x: 6, y: 5, value: 9648110 },
    { name: 'Paris', abbreviation: 'PR', x: 6, y: 6, value: 2161000 },
    { name: 'Tokyo', abbreviation: 'TK', x: 6, y: 7, value: 13960000 },
    { name: 'Sydney', abbreviation: 'SY', x: 6, y: 8, value: 5312163 },

    { name: 'Pennsylvania', abbreviation: 'PA', x: 7, y: 5, value: 30503301 },
    { name: 'Tennessee', abbreviation: 'TN', x: 7, y: 6, value: 7126489 },
    { name: 'Virginia', abbreviation: 'VA', x: 7, y: 7, value: 1770071 },
    { name: 'West Virginia', abbreviation: 'WV', x: 7, y: 8, value: 8715698 },

    { name: 'North Carolina', abbreviation: 'NC', x: 8, y: 5, value: 10835491 },
    { name: 'Wyoming', abbreviation: 'WY', x: 8, y: 6, value: 10835491 },
    { name: 'Illinois', abbreviation: 'IL', x: 8, y: 7, value: 10835491 },
    { name: 'Texas', abbreviation: 'TX', x: 8, y: 8, value: 30503301 },
  ];

  private numericConfigs: NumericChartConfig[] = [
    {
      title: 'Card 5',
      cols: 5,
      rows: 1,
      contentType: 'area-chart',
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
    },
    {
      title: 'Card 7',
      cols: 5,
      rows: 1,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'bar-chart',
    },
    {
      title: 'Card 8',
      cols: 10,
      rows: 2,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'area-chart',
    },
    {
      title: 'Card 9',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 10',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 11',
      cols: 6,
      rows: 1,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 12',
      cols: 4,
      rows: 1,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'line-chart',
    },

    {
      title: 'Card 13',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'line-chart',
    },
    {
      title: 'Card 14',
      cols: 5,
      rows: 2,
      seriesData: [{ data: [3, 8, 1, 10, 5, 2, 7, 4, 9, 6] }],
      contentType: 'line-chart',
    },
  ];

  private honeycombConfigs: HexagonChartConfig[] = [
    {
      title: 'Card 6',
      cols: 5,
      rows: 2,
      seriesData: this.honeycombData,
      contentType: 'honeycomb-chart',
    },
  ];

  chartConfigs: ChartConfig[] = [...this.honeycombConfigs, ...this.numericConfigs];
}
