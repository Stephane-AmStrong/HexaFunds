import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerStatusIndicatorComponent } from './server-status-indicator.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('ServerStatusIndicatorComponent', () => {
  let component: ServerStatusIndicatorComponent;
  let fixture: ComponentFixture<ServerStatusIndicatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServerStatusIndicatorComponent],
      providers: [
        provideZonelessChangeDetection(),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServerStatusIndicatorComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('status', 'Up');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
