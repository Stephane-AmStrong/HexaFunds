import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuffixPictogramComponent } from './suffix-pictogram.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('SuffixPictogramComponent', () => {
  let component: SuffixPictogramComponent;
  let fixture: ComponentFixture<SuffixPictogramComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuffixPictogramComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuffixPictogramComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('icon', 'icon-name');
    fixture.componentRef.setInput('label', 'label-text');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
