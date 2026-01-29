import { ApplicationConfig, DOCUMENT, inject, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { apiErrorInterceptor } from './core/api/api.interceptor';
import { SessionStore } from './core/stores/session.store';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(
      routes,
      withViewTransitions({
        onViewTransitionCreated: ({ from, to }) => {
          const doc = inject(DOCUMENT);
          const fromIndex = (from?.data?.['vtIndex'] as number | undefined) ?? 0;
          const toIndex = (to?.data?.['vtIndex'] as number | undefined) ?? 0;
          doc.documentElement.dataset['navDir'] = toIndex === fromIndex
            ? 'none'
            : toIndex > fromIndex
              ? 'forward'
              : 'back';
          const theme = (to?.data?.['vtTheme'] as string | undefined);
          if (theme) doc.documentElement.dataset['routeTheme'] = theme;
        }
      })),
    provideHttpClient(
      withInterceptors([apiErrorInterceptor])
    ),
    provideAppInitializer(() => {
      inject(SessionStore).loadFromStorage();
    })
  ]
};
