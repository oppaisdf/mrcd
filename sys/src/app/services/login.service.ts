import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { LoginResponse } from '../core/models/responses/login';
import { LoginRequest } from '../core/models/requests/login';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(
    private _api: ApiService
  ) { }

  public async IsLogged() {
    const session = document.cookie.includes('mrcd-session');
    if (session) {
      const response = await this._api.Get<{ db: string }>("Public/Health");
      if (response.success && response.data!.db === "Active") return true;
    }
    return false;
  }

  public async Login(
    loginRequest: LoginRequest
  ) {
    const response = await this._api.Post<LoginResponse>('/Login', loginRequest);

    if (response.success)
      this.SaveRole(response.data!.toString());
    else
      this.RemoveSession();

    return response;
  }

  public async Logout() {
    const response = await this._api.Delete('/Login');
    if (response.success) this.RemoveSession();
    return response;
  }

  public HasUserPermission(
    roleToValuate: string
  ): boolean {
    const cookies = document.cookie.split(';').find(c => c.trim().startsWith('mrcd-roles'));
    if (!cookies) return false;
    const roles = cookies.split('=')[1];
    if (!roles) return false;
    return roles.includes(roleToValuate);
  }

  private SaveRole(
    roles: string
  ) {
    if (document.cookie.includes('mrcd-roles')) return;
    if (!roles) return;
    document.cookie = `mrcd-roles=${roles}; expires=Session; SameSite=None; Secure=true; path=/`;
  }

  private RemoveSession() {
    const cookies = document.cookie.split(';');

    for (let cookie of cookies) {
      const i = cookie.indexOf('=');
      const name = i > -1 ? cookie.substring(0, i) : cookie;
      document.cookie = `${name}=; expires=Mon, 01 Jan 2024 00:00:00 UTC; path=/;`;
    }
  }
}
