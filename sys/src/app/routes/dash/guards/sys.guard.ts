import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { LoginService } from '../../../services/login.service';

export const sysGuard: CanActivateFn = (route, state) => {
  const _service = inject(LoginService);
  return _service.HasUserPermission('sys');
};
