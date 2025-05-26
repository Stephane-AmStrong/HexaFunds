import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SavingsAccountListComponent } from '../../savings-account/savings-account-list/savings-account-list.component';

describe('SavingsAccountListComponent', () => {
  let component: SavingsAccountListComponent;
  let fixture: ComponentFixture<SavingsAccountListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SavingsAccountListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SavingsAccountListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
