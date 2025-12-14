import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionServicesComponent } from './section-services.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHighcharts } from 'highcharts-angular';

describe('SectionServicesComponent', () => {
  let component: SectionServicesComponent;
  let fixture: ComponentFixture<SectionServicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionServicesComponent],
      providers: [
        provideZonelessChangeDetection(),
        provideHighcharts()
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SectionServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
