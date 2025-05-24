import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckingAccountDetailComponent } from './checking-account-detail.component';

describe('CheckingAccountDetailComponent', () => {
  let component: CheckingAccountDetailComponent;
  let fixture: ComponentFixture<CheckingAccountDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CheckingAccountDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CheckingAccountDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
