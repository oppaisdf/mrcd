import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const detailGuard: CanActivateFn = (route, state) => {
  const _router = inject(Router);
  const id = route.paramMap.get('id');
  if (!id) {
    _router.navigateByUrl('/parent/all');
    return false;
  }

  const isNumber = !isNaN(+id);
  if (isNumber) return true;
  _router.navigateByUrl('/parent/all');
  return false;
};
