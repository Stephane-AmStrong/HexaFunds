import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckingAccountListComponent } from './checking-account-list.component';

describe('CheckingAccountListComponent', () => {
  let component: CheckingAccountListComponent;
  let fixture: ComponentFixture<CheckingAccountListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CheckingAccountListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CheckingAccountListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
