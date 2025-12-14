import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConnectionListComponent } from './connection-list.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { BaseApiService } from '../../../core/services/base-api.service';
import { ConnectionService } from '../services/connection.service';
import { ActivatedRoute } from '@angular/router';

describe('ConnectionListComponent', () => {
  let component: ConnectionListComponent;
  let fixture: ComponentFixture<ConnectionListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConnectionListComponent],
      providers: [
        ConnectionService,
        BaseApiService,
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
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConnectionListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
