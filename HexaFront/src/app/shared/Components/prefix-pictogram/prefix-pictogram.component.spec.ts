import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrefixPictogramComponent } from './prefix-pictogram.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('PrefixPictogramComponent', () => {
  let component: PrefixPictogramComponent;
  let fixture: ComponentFixture<PrefixPictogramComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PrefixPictogramComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrefixPictogramComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('icon', 'icon-name');
    fixture.componentRef.setInput('label', 'label-text');

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
