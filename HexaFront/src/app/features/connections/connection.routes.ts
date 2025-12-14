import { Routes } from '@angular/router';
import { ConnectionListComponent } from './connection-list/connection-list.component';
import { APP_ROUTES } from '../../core/constants/app-routes';

export const connectionRoutes: Routes = [
  {
    path: APP_ROUTES.CONNECTIONS.LIST,
    component: ConnectionListComponent,
  },
];
