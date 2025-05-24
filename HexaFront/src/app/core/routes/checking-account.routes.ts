import { Routes } from '@angular/router';
import { CheckingAccountDetailComponent } from '../../features/banking/Components/checking-account/checking-account-detail/checking-account-detail.component';
import { checkingAccountResolver } from '../../features/banking/resolvers/checking-account.resolver';
import { transactionsOfAccountResolver } from '../../features/banking/resolvers/transaction.resolver';
import { CheckingAccountListComponent } from '../../features/banking/Components/checking-account/checking-account-list/checking-account-list.component';

export const checkingAccountRoutes: Routes = [
  {
    path: '',
    component: CheckingAccountListComponent,
  },
  {
    path: ':id',
    component: CheckingAccountDetailComponent,
    resolve: {
      checkingAccount: checkingAccountResolver,
      transactions: transactionsOfAccountResolver,
    },
  },
];
