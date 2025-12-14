import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HostListComponent } from './host-list.component';
import { provideZonelessChangeDetection } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { BaseApiService } from '../../../core/services/base-api.service';
import { HostService } from '../services/host.service';
import { ActivatedRoute } from '@angular/router';

describe('HostListComponent', () => {
  let component: HostListComponent;
  let fixture: ComponentFixture<HostListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HostListComponent],
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

    fixture = TestBed.createComponent(HostListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
