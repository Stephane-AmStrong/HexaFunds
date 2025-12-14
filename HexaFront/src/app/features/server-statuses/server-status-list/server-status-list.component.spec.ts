import { ComponentFixture, TestBed } from '@angular/core/testing';

import { provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { BaseApiService } from '../../../core/services/base-api.service';
import { ServerStatusListComponent } from './server-status-list.component';
import { ServerStatusService } from '../services/server-status.service';
import { ActivatedRoute } from '@angular/router';

describe('ServerStatusListComponent', () => {
  let component: ServerStatusListComponent;
  let fixture: ComponentFixture<ServerStatusListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServerStatusListComponent],
      providers: [
        ServerStatusService,
        BaseApiService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection(),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                withName: 'test-host',
                withAppName: 'test-app',
                withVersion: '1.0.0',
              },
              queryParams: {
                searchTerm: '',
                orderBy: 'name',
                page: 1,
                pageSize: 10,
              },
            },
          },
        },
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ServerStatusListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
