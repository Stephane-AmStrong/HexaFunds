import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AreaChartComponent } from './area-chart.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHighcharts } from 'highcharts-angular';
import { MOCK_NumericChartConfig } from '../Mock_ChartConfig';

describe('AreaChartComponent', () => {
  let component: AreaChartComponent;
  let fixture: ComponentFixture<AreaChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AreaChartComponent],
      providers: [provideZonelessChangeDetection(), provideHighcharts()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AreaChartComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('chartConfig', MOCK_NumericChartConfig);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
