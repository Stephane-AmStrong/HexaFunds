import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerTableComponent } from './server-table.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('ServerTableComponent', () => {
  let component: ServerTableComponent;
  let fixture: ComponentFixture<ServerTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServerTableComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServerTableComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('servers', []);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
