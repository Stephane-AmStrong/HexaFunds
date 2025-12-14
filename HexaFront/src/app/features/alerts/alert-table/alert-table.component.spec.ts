import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertTableComponent } from './alert-table.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('AlertTableComponent', () => {
  let component: AlertTableComponent;
  let fixture: ComponentFixture<AlertTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AlertTableComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlertTableComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('alerts', []);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
