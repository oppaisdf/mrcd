import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { SessionStore } from '../stores/session.store';

export const guestGuard: CanMatchFn = (route, segments) => {
  const session = inject(SessionStore);
  const router = inject(Router);
  return session.isAuthenticated()
    ? router.createUrlTree(['/'])
    : true;
};
