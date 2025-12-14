import { Injectable } from '@angular/core';
import Highcharts from 'highcharts';

@Injectable({
  providedIn: 'root',
})
export class HighchartsThemeService {
  getThemeOptions(): Highcharts.Options {
    return {
      chart: {
        backgroundColor: 'transparent',
      },
      accessibility: {
        enabled: false
      },
      title: {
        style: {
          color: 'var(--mat-sys-on-surface)',
          fontSize: 'var(--mat-sys-typescale-headline-small-size, 16px)',
          fontWeight: 'var(--mat-sys-typescale-headline-small-weight, 500)',
          fontFamily: 'var(--mat-sys-typeface-brand, Roboto, "Helvetica Neue", sans-serif)',
        },
      },
      legend: {
        enabled: false,
        itemStyle: {
          color: 'var(--mat-sys-on-surface-variant)',
          fontSize: 'var(--mat-sys-typescale-body-medium-size, 14px)',
          fontFamily: 'var(--mat-sys-typeface-brand, Roboto, "Helvetica Neue", sans-serif)',
        },
      },
      xAxis: {
        lineColor: 'var(--mat-sys-outline)',
        tickColor: 'var(--mat-sys-outline)',
        labels: {
          style: {
            color: 'var(--mat-sys-on-surface-variant)',
            fontSize: 'var(--mat-sys-typescale-label-small-size, 11px)',
            fontFamily: 'var(--mat-sys-typeface-brand, Roboto, "Helvetica Neue", sans-serif)',
          },
        },
      },
      yAxis: {
        gridLineColor: 'var(--mat-sys-outline-variant)',
        labels: {
          style: {
            color: 'var(--mat-sys-on-surface-variant)',
            fontSize: 'var(--mat-sys-typescale-label-small-size, 11px)',
            fontFamily: 'var(--mat-sys-typeface-brand, Roboto, "Helvetica Neue", sans-serif)',
          },
        },
      },
      plotOptions: {
        series: {
          color: 'var(--mat-sys-primary)',
        },
        line: {
          marker: {
            fillColor: 'var(--mat-sys-primary)',
            lineColor: 'var(--mat-sys-primary)',
          },
        },
      },
    };
  }

  getModules() {
    return [
      import('highcharts/modules/map'),
      import('highcharts/modules/heatmap'),
      import('highcharts/modules/tilemap'),
      import('highcharts/modules/exporting'),
      import('highcharts/modules/export-data'),
      import('highcharts/themes/adaptive'),
    ];
  }
}
