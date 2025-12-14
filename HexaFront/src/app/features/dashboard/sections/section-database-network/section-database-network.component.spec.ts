import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SectionDatabaseNetworkComponent } from './section-database-network.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('SectionDatabaseNetworkComponent', () => {
  let component: SectionDatabaseNetworkComponent;
  let fixture: ComponentFixture<SectionDatabaseNetworkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SectionDatabaseNetworkComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SectionDatabaseNetworkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
