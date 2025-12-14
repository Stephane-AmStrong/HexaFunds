import { Routes } from '@angular/router';
import { ServerListComponent } from './server-list/server-list.component';
import { APP_ROUTES } from '../../core/constants/app-routes';

export const serverRoutes: Routes = [
  {
    path: APP_ROUTES.SERVERS.LIST,
    component: ServerListComponent,
  },
];
