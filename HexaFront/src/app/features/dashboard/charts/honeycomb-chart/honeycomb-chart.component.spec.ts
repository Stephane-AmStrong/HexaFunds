import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HoneycombChartComponent } from './honeycomb-chart.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { MOCK_HexagonChartConfig } from '../Mock_ChartConfig';
import { provideHighcharts } from 'highcharts-angular';

describe('HoneycombChartComponent', () => {
  let component: HoneycombChartComponent;
  let fixture: ComponentFixture<HoneycombChartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HoneycombChartComponent],
      providers: [provideZonelessChangeDetection(), provideHighcharts()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HoneycombChartComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('chartConfig', MOCK_HexagonChartConfig);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
