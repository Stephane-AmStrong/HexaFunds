import { Routes } from '@angular/router';
import { TransactionDetailComponent } from '../../features/banking/Components/transaction-detail/transaction-detail.component';
import { TransactionListComponent } from '../../features/banking/Components/transaction/transaction-list/transaction-list.component';

export const transactionRoutes: Routes = [
  {
    path: '',
    component: TransactionListComponent,
  },
  {
    path: ':id',
    component: TransactionDetailComponent,
  },
];
