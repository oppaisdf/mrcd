import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ApiService } from '../../services/api.service';

export const loginGuard: CanActivateFn = async (route, state) => {
  const _api = inject(ApiService);
  const _router = inject(Router);

  const session = document.cookie.includes('kmd-session');
  if (session) {
    const response = await _api.Get<{ db: string }>("Public/Health");
    if (response.success && response.data!.db === "Active") return true;
  }

  _router.navigateByUrl("/login");
  return false;
};
