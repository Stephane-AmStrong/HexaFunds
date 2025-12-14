import { Routes } from '@angular/router';
import { AlertListComponent } from './alert-list/alert-list.component';
import { APP_ROUTES } from '../../core/constants/app-routes';

export const alertRoutes: Routes = [
  {
    path: APP_ROUTES.ALERTS.LIST,
    component: AlertListComponent,
  },
];
