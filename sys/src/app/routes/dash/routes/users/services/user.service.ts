import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { UserResponse } from '../models/responses/user';
import { UserRequest } from '../models/requests/user';
import { RoleResponse } from '../../roles/responses/role';

@Injectable()
export class UserService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetRolesAsync() {
    return await this._api.Get<RoleResponse[]>('Role');
  }

  public async GetAsync() {
    return await this._api.Get<UserResponse[]>('User');
  }

  public async GetByIdAsync(
    id: string
  ) {
    return await this._api.Get<UserResponse>(`User/${id}`);
  }

  public async CreateAsync(
    request: UserRequest
  ) {
    return await this._api.Post<{ id: string }>('User', request);
  }

  public async UpdateAsync(
    id: string,
    request: UserRequest
  ) {
    return await this._api.Patch(`User/${id}`, request);
  }
}
