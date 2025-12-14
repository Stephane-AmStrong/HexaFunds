import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientListComponent } from './client-list.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { BaseApiService } from '../../../core/services/base-api.service';
import { ClientService } from '../services/client.service';
import { ActivatedRoute } from '@angular/router';

describe('ClientListComponent', () => {
  let component: ClientListComponent;
  let fixture: ComponentFixture<ClientListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientListComponent],
      providers: [
        ClientService,
        BaseApiService,
        provideHttpClient(),
        provideHttpClientTesting(),
        provideZonelessChangeDetection(),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                withGaia: 'test-host',
                withLogin: 'test-app',
                searchTerm: '1.0.0',
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
    }).compileComponents();

    fixture = TestBed.createComponent(ClientListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
