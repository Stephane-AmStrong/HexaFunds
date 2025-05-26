import { Routes } from '@angular/router';
import { TransactionListComponent } from '../../features/banking/Components/transaction/transaction-list/transaction-list.component';

export const transactionRoutes: Routes = [
  {
    path: '',
    component: TransactionListComponent,
  },
];
