import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { SessionStore } from '../stores/session.store';

export const roleGuard = (...requiredRoles: string[]): CanMatchFn => {
  return () => {
    const session = inject(SessionStore);
    const router = inject(Router);
    if (!session.isAuthenticated())
      return router.createUrlTree(['/login'], { queryParams: { returnUrl: '/' } });
    const ok = requiredRoles.some(r => session.hasRole(r));
    return ok ? true : router.createUrlTree(['/forbidden']);
  };
};
