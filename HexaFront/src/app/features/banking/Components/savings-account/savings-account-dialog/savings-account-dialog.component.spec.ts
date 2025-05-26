import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SavingsAccountDialogComponent } from './savings-account-dialog.component';

describe('SavingsAccountDialogComponent', () => {
  let component: SavingsAccountDialogComponent;
  let fixture: ComponentFixture<SavingsAccountDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SavingsAccountDialogComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SavingsAccountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
