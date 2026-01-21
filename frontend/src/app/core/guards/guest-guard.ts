import { Inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { SessionStore } from '../stores/session.store';

export const guestGuard: CanMatchFn = (route, segments) => {
  const session = Inject(SessionStore);
  const router = Inject(Router);
  return session.isAuthenticated()
    ? router.createUrlTree(['/'])
    : true;
};
