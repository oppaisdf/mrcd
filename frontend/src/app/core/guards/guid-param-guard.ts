import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { isGuid } from '../utils/guid';

export const guidParamGuard = (paramName: string): CanMatchFn => (route, segments) => {
  const router = inject(Router);
  const paramIndex = route.path
    ?.split('/')
    .findIndex(p => p === `:${paramName}`);

  if (paramIndex == null || paramIndex < 0)
    return false;

  const value = segments[paramIndex]?.path;

  if (!value || !isGuid(value))
    return router.createUrlTree(['/not-found']);
  else return true;
};
