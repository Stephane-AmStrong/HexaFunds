import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LineChartComponent } from './line-chart.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHighcharts } from 'highcharts-angular';
import { MOCK_NumericChartConfig } from '../Mock_ChartConfig';

describe('LineChartComponent', () => {
  let component: LineChartComponent;
  let fixture: ComponentFixture<LineChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LineChartComponent],
      providers: [provideZonelessChangeDetection(), provideHighcharts()],
    }).compileComponents();

    fixture = TestBed.createComponent(LineChartComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('chartConfig', MOCK_NumericChartConfig);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
