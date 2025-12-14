import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { HighchartsChartComponent, ChartConstructorType } from 'highcharts-angular';
import {  NumericChartConfig } from '../../chart-config';

@Component({
  selector: 'area-chart',
  imports: [HighchartsChartComponent, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './area-chart.component.html',
  styleUrl: './area-chart.component.scss',
})
export class AreaChartComponent {
  chartConfig = input.required<NumericChartConfig>();

  get chartOptions(): Highcharts.Options {
    return {
      title: {
        text: this.chartConfig().title,
      },
      series: [
        {
          name: 'Jane',
          data: [3, 8, 1, 4, 9, 6, 10, 5, 6],
          type: 'area',
        },
        {
          name: 'John',
          data: [8, 7, 2, 10, 5, 6],
          type: 'area',
        },
      ],
    };
  }

  chartConstructor: ChartConstructorType = 'chart';
  updateFlag: boolean = false;
  oneToOneFlag: boolean = true;
}
