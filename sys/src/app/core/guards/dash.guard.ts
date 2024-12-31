import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { LoginService } from '../../services/login.service';

export const dashGuard: CanActivateFn = async (route, state) => {
  const _login = inject(LoginService);
  const _router = inject(Router);

  if (await _login.IsLogged()) return true;

  _router.navigateByUrl("/login");
  return false;
};
