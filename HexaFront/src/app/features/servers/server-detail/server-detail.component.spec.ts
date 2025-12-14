import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerDetailComponent } from './server-detail.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { MOCK_SERVERS } from '../MOCK_SERVERS';
import { ActivatedRoute } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('ServerDetailComponent', () => {
  let component: ServerDetailComponent;
  let fixture: ComponentFixture<ServerDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServerDetailComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection(),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {},
              queryParams: {},
            },
          },
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(ServerDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});