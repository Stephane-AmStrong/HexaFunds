import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarChartComponent } from './bar-chart.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHighcharts } from 'highcharts-angular';
import { MOCK_NumericChartConfig } from '../Mock_ChartConfig';

describe('BarChartComponent', () => {
  let component: BarChartComponent;
  let fixture: ComponentFixture<BarChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BarChartComponent],
      providers: [provideZonelessChangeDetection(), provideHighcharts()],
    }).compileComponents();

    fixture = TestBed.createComponent(BarChartComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('chartConfig', MOCK_NumericChartConfig);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
