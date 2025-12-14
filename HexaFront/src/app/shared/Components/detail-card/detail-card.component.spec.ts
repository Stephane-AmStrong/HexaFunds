import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailCardComponent } from './detail-card.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('DetailCardComponent', () => {
  let component: DetailCardComponent;
  let fixture: ComponentFixture<DetailCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailCardComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
