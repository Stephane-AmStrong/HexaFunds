import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostDetailComponent } from './host-detail.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideZonelessChangeDetection } from '@angular/core';
import { BaseApiService } from '../../../core/services/base-api.service';
import { HostService } from '../services/host.service';
import { ActivatedRoute } from '@angular/router';

describe('HostDetailComponent', () => {
  let component: HostDetailComponent;
  let fixture: ComponentFixture<HostDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostDetailComponent],
      providers: [
        HostService,
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

    fixture = TestBed.createComponent(HostDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
