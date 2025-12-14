import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerCardComponent } from './server-card.component';
import { provideZonelessChangeDetection } from '@angular/core';

describe('ServerCardComponent', () => {
  let component: ServerCardComponent;
  let fixture: ComponentFixture<ServerCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServerCardComponent],
      providers: [provideZonelessChangeDetection()]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServerCardComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('server', {} as any);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
