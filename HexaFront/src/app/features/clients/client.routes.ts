import { Routes } from '@angular/router';
import { ClientListComponent } from './client-list/client-list.component';
import { APP_ROUTES } from '../../core/constants/app-routes';
import { ClientDetailComponent } from './client-detail/client-detail.component';

export const clientRoutes: Routes = [
  {
    path: APP_ROUTES.CLIENTS.LIST,
    component: ClientListComponent,
  },
  {
    path: APP_ROUTES.CLIENTS.DETAIL,
    component: ClientDetailComponent,
  }
];