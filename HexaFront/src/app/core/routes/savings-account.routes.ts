import { Routes } from '@angular/router';
import { SavingsAccountListComponent } from '../../features/banking/Components/savings-account/savings-account-list/savings-account-list.component';
import { savingsAccountResolver } from '../../features/banking/resolvers/savings-account.resolver';
import { transactionsOfAccountResolver } from '../../features/banking/resolvers/transaction.resolver';
import { SavingsAccountDetailComponent } from '../../features/banking/Components/savings-account/savings-account-detail/savings-account-detail.component';

export const savingsAccountRoutes: Routes = [
  {
    path: '',
    component: SavingsAccountListComponent,
  },
  {
    path: ':id',
    component: SavingsAccountDetailComponent,
    resolve: {
      savingsAccount: savingsAccountResolver,
      transactions: transactionsOfAccountResolver,
    },
  },
];
