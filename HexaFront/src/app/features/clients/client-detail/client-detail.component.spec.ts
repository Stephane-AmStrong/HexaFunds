import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientDetailComponent } from './client-detail.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { MOCK_CLIENTS } from '../MOCK_CLIENTS';
import { ActivatedRoute } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { BaseApiService } from '../../../core/services/base-api.service';
import { ClientService } from '../services/client.service';

describe('ClientDetailComponent', () => {
  let component: ClientDetailComponent;
  let fixture: ComponentFixture<ClientDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClientDetailComponent],
      providers: [
        BaseApiService,
        ClientService,
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

    fixture = TestBed.createComponent(ClientDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});