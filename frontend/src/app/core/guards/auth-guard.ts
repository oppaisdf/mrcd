import { Inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { SessionStore } from '../stores/session.store';

export const authGuard: CanMatchFn = (route, segments) => {
  const session = Inject(SessionStore);
  const router = Inject(Router);
  if (session.isAuthenticated()) return true;
  const returnUrl = '/' + segments.map(s => s.path).join('/');
  return router.createUrlTree(['/login'], { queryParams: { returnUrl } });
};
