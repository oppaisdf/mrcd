import { CanActivateFn, Router } from '@angular/router';
import { LoginService } from '../../services/login.service';
import { inject } from '@angular/core';

export const loginGuard: CanActivateFn = async (route, state) => {
  const _login = inject(LoginService);
  const _router = inject(Router);

  if (!await _login.IsLogged()) return true;

  _router.navigateByUrl('/dash');
  return false;
};
