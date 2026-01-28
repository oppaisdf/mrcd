import { inject } from '@angular/core';
import { CanMatchFn, Route, Router, UrlSegment } from '@angular/router';
import { SessionStore } from '../stores/session.store';

export const roleGuard: CanMatchFn = (route: Route, segments: UrlSegment[]) => {
  const session = inject(SessionStore);
  const router = inject(Router);

  if (!session.isAuthenticated())
    return router.createUrlTree(['/login'], { queryParams: { returnUrl: '/' } });

  const requiredRoles = route.data?.['roles'] as string[] | undefined;
  if (!requiredRoles || requiredRoles.length === 0)
    return false;

  const ok = requiredRoles.some(r => session.hasRole(r));
  return ok ? true : router.createUrlTree(['/forbidden']);
};
