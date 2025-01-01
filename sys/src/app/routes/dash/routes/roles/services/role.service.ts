import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { RoleResponse } from '../models/responses/role';
import { RoleRequest } from '../models/requests/role';

@Injectable()
export class RoleService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetAsync() {
    return await this._api.Get<RoleResponse[]>('Role');
  }

  public async AddAsync(
    request: RoleRequest
  ) {
    return await this._api.Post('Role', request);
  }
}
