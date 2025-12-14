import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerStatusTableComponent } from './server-status-table.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('ServerStatusTableComponent', () => {
  let component: ServerStatusTableComponent;
  let fixture: ComponentFixture<ServerStatusTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServerStatusTableComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServerStatusTableComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('serverStatuses', []);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
