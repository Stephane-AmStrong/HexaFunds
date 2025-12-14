import { Routes } from '@angular/router';
import { APP_ROUTES } from '../../core/constants/app-routes';
import { ErrorMessageComponent } from './error-message/error-message.component';
import { ERROR_CATALOG } from './error-catalog';

export const errorRoutes: Routes = [
  {
    path: APP_ROUTES.ERRORS.UNAUTHORIZED,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['401'],
  },
  {
    path: APP_ROUTES.ERRORS.FORBIDDEN,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['403'],
  },
  {
    path: APP_ROUTES.ERRORS.RESOURCE_NOT_FOUND,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['404'],
  },
  {
    path: APP_ROUTES.ERRORS.INTERNAL_SERVER_ERROR,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['500'],
  },
  {
    path: APP_ROUTES.ERRORS.BAD_GATEWAY,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['502'],
  },
  {
    path: APP_ROUTES.ERRORS.SERVICE_UNAVAILABLE,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['503'],
  },
  {
    path: APP_ROUTES.ERRORS.RATE_LIMIT,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['429'],
  },
  {
    path: APP_ROUTES.ERRORS.ROOT,
    redirectTo: APP_ROUTES.ERRORS.FORBIDDEN,
    pathMatch: 'full',
  },
  {
    path: APP_ROUTES.ERRORS.UNKNOWN,
    component: ErrorMessageComponent,
    data: ERROR_CATALOG['unknown'],
  },
];
