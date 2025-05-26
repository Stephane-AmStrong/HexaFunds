import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckingAccountDialogComponent } from './checking-account-dialog.component';

describe('CheckingAccountDialogComponent', () => {
  let component: CheckingAccountDialogComponent;
  let fixture: ComponentFixture<CheckingAccountDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CheckingAccountDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CheckingAccountDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
