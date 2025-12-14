import { Routes } from '@angular/router';
import { APP_ROUTES } from '../../core/constants/app-routes';
import { ServerStatusListComponent } from './server-status-list/server-status-list.component';

export const serverstatusRoutes: Routes = [
  {
    path: APP_ROUTES.SERVERS_STATUS.LIST,
    component: ServerStatusListComponent,
  },
];
