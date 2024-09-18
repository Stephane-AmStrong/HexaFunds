import { Routes } from '@angular/router';
import { SavingsAccountComponent } from '../../features/banking/Components/savings-account/savings-account.component';
import { SavingsAccountDetailComponent } from '../../features/banking/Components/savings-account-detail/savings-account-detail.component';

export const savingsAccountRoutes: Routes = [
  {
    path: '',
    component: SavingsAccountComponent,
  },
  {
    path: ':id',
    component: SavingsAccountDetailComponent,
  },
];
