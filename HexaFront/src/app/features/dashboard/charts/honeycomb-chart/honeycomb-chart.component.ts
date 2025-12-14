import { Component, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { ChartConstructorType, HighchartsChartComponent } from 'highcharts-angular';
import Highcharts from 'highcharts';
import { HexagonChartConfig } from '../../chart-config';

@Component({
  selector: 'honeycomb-chart',
  imports: [HighchartsChartComponent, MatMenuModule, MatIconModule, MatButtonModule, MatCardModule],
  templateUrl: './honeycomb-chart.component.html',
  styleUrl: './honeycomb-chart.component.scss',
})
export class HoneycombChartComponent {
  chartConfig = input.required<HexagonChartConfig>();
  chartConstructor: ChartConstructorType = 'mapChart';
  updateFlag: boolean = false;
  oneToOneFlag: boolean = true;
  get chartOptions(): Highcharts.Options {
    return {
    chart: {
      type: 'tilemap',
      inverted: true,
    },

    title: {
      text: this.chartConfig().title,
    },

    xAxis: {
      visible: false,
    },

    yAxis: {
      visible: false,
    },

    colorAxis: {
      dataClasses: [
        { from: 0, to: 5000000, color: '#FFD700', name: '< 5M' },
        { from: 5000000, to: 20000000, color: '#4CAF50', name: '5M - 20M' },
        { from: 20000000, color: '#E53935', name: '> 20M' },
      ],
    },

    tooltip: {
      headerFormat: '',
      pointFormat: 'The population of <b>{point.name}</b> is <b>{point.value}</b>',
    },

    plotOptions: {
      series: {
        dataLabels: {
          enabled: true,
          format: '{point.abbreviation}',
          style: { textOutline: 'false' },
        },
      },
    },

    series: [
      {
        type: 'tilemap',
        data: this.chartConfig().seriesData?.map(series => ({
          name: series.name,
          abbreviation: series.abbreviation,
          x: series.x,
          y: series.y,
          value: series.value
        }))
      },
    ],
    };
  }
}
