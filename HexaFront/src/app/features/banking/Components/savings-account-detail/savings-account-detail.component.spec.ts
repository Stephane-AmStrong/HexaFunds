import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SavingsAccountDetailComponent } from './savings-account-detail.component';

describe('SavingsAccountDetailComponent', () => {
  let component: SavingsAccountDetailComponent;
  let fixture: ComponentFixture<SavingsAccountDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SavingsAccountDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SavingsAccountDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
