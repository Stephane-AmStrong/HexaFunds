import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientCardComponent } from './client-card.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('ClientCardComponent', () => {
  let component: ClientCardComponent;
  let fixture: ComponentFixture<ClientCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientCardComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientCardComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('client', {} as any);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
