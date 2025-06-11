import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const detailGuard: CanActivateFn = (route, state) => {
  const _router = inject(Router);

  const id = route.paramMap.get("id");
  if (!id) return false;
  const isValidId = !isNaN(+id);
  if (isValidId) return true;
  _router.navigateByUrl('/charge/all');
  return false;
};
