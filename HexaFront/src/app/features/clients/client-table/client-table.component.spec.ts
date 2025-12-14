import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientTableComponent } from './client-table.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('ClientTableComponent', () => {
  let component: ClientTableComponent;
  let fixture: ComponentFixture<ClientTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientTableComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClientTableComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('clients', []);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
