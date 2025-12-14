import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZonelessChangeDetection,
} from '@angular/core';
import { provideRouter, withComponentInputBinding, withRouterConfig } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpInterceptor } from './core/interceptors/http.interceptor';
import { provideHighcharts } from 'highcharts-angular';
import { HighchartsThemeService } from './features/dashboard/services/highcharts-theme.service';
import { httpErrorInterceptorInterceptor } from './core/interceptors/http-error-interceptor.interceptor';
import Highcharts from 'highcharts';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideHttpClient(withInterceptors([httpInterceptor, httpErrorInterceptorInterceptor])),
    provideRouter(
      routes,
      withComponentInputBinding(),
      withRouterConfig({ paramsInheritanceStrategy: 'always' })
    ),
    provideHighcharts({
      instance: () => Promise.resolve(Highcharts),
      options: new HighchartsThemeService().getThemeOptions(),
      modules: () => new HighchartsThemeService().getModules(),
    }),
  ],
};
