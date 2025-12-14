import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionApplicationsComponent } from './section-applications.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('SectionApplicationsComponent', () => {
  let component: SectionApplicationsComponent;
  let fixture: ComponentFixture<SectionApplicationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionApplicationsComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SectionApplicationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
