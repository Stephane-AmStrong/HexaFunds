import { Routes } from '@angular/router';
import { HostListComponent } from './host-list/host-list.component';
import { APP_ROUTES } from '../../core/constants/app-routes';
import { ServerDetailComponent } from '../servers/server-detail/server-detail.component';
import { HostDetailComponent } from './host-detail/host-detail.component';

export const hostRoutes: Routes = [
  {
    path: APP_ROUTES.HOSTS.LIST,
    component: HostListComponent,
  },
  {
    path: APP_ROUTES.HOSTS.DETAIL,
    component: HostDetailComponent,
  },
  {
    path: APP_ROUTES.HOSTS.SERVERS_REDIRECT,
    redirectTo: APP_ROUTES.HOSTS.DETAIL,
    pathMatch: 'full'
  },
  {
    path: APP_ROUTES.HOSTS.SERVER_BY_HOST,
    component: ServerDetailComponent,
  }
];
