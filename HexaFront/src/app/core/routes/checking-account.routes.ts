import { Routes } from '@angular/router';
import { CheckingAccountComponent } from '../../features/banking/Components/checking-account/checking-account.component';
import { CheckingAccountDetailComponent } from '../../features/banking/Components/checking-account-detail/checking-account-detail.component';
import { checkingAccountResolver } from '../../features/banking/resolvers/checking-account.resolver';

export const checkingAccountRoutes: Routes = [
  {
    path: '',
    component: CheckingAccountComponent,
  },
  {
    path: ':id',
    component: CheckingAccountDetailComponent,
    resolve: {
      checkingAccount: checkingAccountResolver
    }
  },
];
