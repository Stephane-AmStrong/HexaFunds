import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutComponent } from './layout.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('LayoutComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LayoutComponent],
      providers: [
        provideZonelessChangeDetection(),
      ]
    }).compileComponents();
  });

  it('should create the layoutComponent', () => {
    const fixture = TestBed.createComponent(LayoutComponent);
    const layoutComponent = fixture.componentInstance;
    expect(layoutComponent).toBeTruthy();
  });
});
