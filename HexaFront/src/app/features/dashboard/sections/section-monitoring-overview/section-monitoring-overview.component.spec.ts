import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionMonitoringOverviewComponent } from './section-monitoring-overview.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('SectionMonitoringOverviewComponent', () => {
  let component: SectionMonitoringOverviewComponent;
  let fixture: ComponentFixture<SectionMonitoringOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionMonitoringOverviewComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SectionMonitoringOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
