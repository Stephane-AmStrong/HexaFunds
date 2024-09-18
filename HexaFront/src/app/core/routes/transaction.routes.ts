import { Routes } from '@angular/router';
import { TransactionComponent } from '../../features/banking/Components/transaction/transaction.component';
import { TransactionDetailComponent } from '../../features/banking/Components/transaction-detail/transaction-detail.component';

export const transactionRoutes: Routes = [
  {
    path: '',
    component: TransactionComponent,
  },
  {
    path: ':id',
    component: TransactionDetailComponent,
  },
];
