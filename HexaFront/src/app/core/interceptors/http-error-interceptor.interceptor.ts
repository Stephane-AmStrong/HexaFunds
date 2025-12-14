import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, tap, throwError } from 'rxjs';
import { ERROR_DYNAMIC_ROUTES, DEFAULT_ERROR_DYNAMIC_ROUTE } from '../../features/errors/error-catalog';

export const httpErrorInterceptorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  return next(req).pipe(
    tap({
      error: (error: HttpErrorResponse) => {
        const route = ERROR_DYNAMIC_ROUTES[error.status] ?? DEFAULT_ERROR_DYNAMIC_ROUTE;
        router.navigate([route]);
      },
    }),
    catchError((error) => {
      return throwError(() => error);
    })
  );
};
