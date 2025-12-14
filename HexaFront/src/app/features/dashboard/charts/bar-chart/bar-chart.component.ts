import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { HighchartsChartComponent, ChartConstructorType } from 'highcharts-angular';
import { NumericChartConfig } from '../../chart-config';

@Component({
  selector: 'bar-chart',
  imports: [HighchartsChartComponent, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './bar-chart.component.html',
  styleUrl: './bar-chart.component.scss'
})
export class BarChartComponent {
  chartConfig = input.required<NumericChartConfig>();

  get chartOptions(): Highcharts.Options {
    return {
      title: {
        text: this.chartConfig().title,
      },
      series: this.chartConfig().seriesData?.map(series =>({
        name: series.name,
        data: series.data,
        type: 'bar',
      })),
    };
  }

  chartConstructor: ChartConstructorType = 'chart';
  updateFlag: boolean = false;
  oneToOneFlag: boolean = true;
}