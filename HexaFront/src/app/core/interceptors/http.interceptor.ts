import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const clonedRequest = req.clone({
    url: `${environment.apiUrl}/${req.url}`,
  })

  return next(clonedRequest);
};
