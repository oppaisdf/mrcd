import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LoginService } from '../../../services/login.service';

export const admGuard: CanActivateFn = (route, state) => {
  const _router = inject(Router);
  const _service = inject(LoginService);
  const hasPermission = _service.HasUserPermission('adm');
  if (!hasPermission) _router.navigateByUrl('');
  return hasPermission;
};
