import { Routes } from '@angular/router';
import { AddressFormComponent } from '../../features/banking/Components/address-form/address-form.component';
import { DashboardComponent } from '../../features/banking/Components/dashboard/dashboard.component';
import { PageNotFoundComponent } from '../../shared/Components/page-not-found/page-not-found.component';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: '/dashboard',
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
  },
  {
    path: 'checking-accounts',
    loadChildren: () =>
      import('./checking-account.routes').then((r) => r.checkingAccountRoutes),
  },
  {
    path: 'savings-accounts',
    loadChildren: () =>
      import('./savings-account.routes').then((r) => r.savingsAccountRoutes),
  },
  {
    path: 'transactions',
    loadChildren: () =>
      import('./transaction.routes').then((r) => r.transactionRoutes),
  },
  {
    path: 'address-form',
    component: AddressFormComponent,
  },
  {
    path: '**',
    component: PageNotFoundComponent,
  },
];
