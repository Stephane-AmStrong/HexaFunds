import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { HighchartsChartComponent, ChartConstructorType } from 'highcharts-angular';
import { NumericChartConfig } from '../../chart-config';

@Component({
  selector: 'line-chart',
  imports: [HighchartsChartComponent, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './line-chart.component.html',
  styleUrl: './line-chart.component.scss'
})
export class LineChartComponent {
  chartConfig = input.required<NumericChartConfig>();

  get chartOptions(): Highcharts.Options {
    return {
      title: {
        text: this.chartConfig().title,
      },
      series: this.chartConfig().seriesData?.map(series => ({
        name: series.name,
        data: series.data,
        type: 'line',
      })),
    };
  }

  chartConstructor: ChartConstructorType = 'chart';
  updateFlag: boolean = false;
  oneToOneFlag: boolean = true;
}
