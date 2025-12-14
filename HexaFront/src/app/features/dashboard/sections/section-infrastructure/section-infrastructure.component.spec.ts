import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionInfrastructureComponent } from './section-infrastructure.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('SectionInfrastructureComponent', () => {
  let component: SectionInfrastructureComponent;
  let fixture: ComponentFixture<SectionInfrastructureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionInfrastructureComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SectionInfrastructureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
