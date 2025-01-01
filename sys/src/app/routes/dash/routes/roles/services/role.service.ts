import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { RoleResponse } from '../responses/role';
import { DefaultRequest } from '../../../models/Request';

@Injectable()
export class RoleService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetAsync() {
    return await this._api.Get<RoleResponse[]>('Role');
  }

  public async AddAsync(
    request: DefaultRequest
  ) {
    return await this._api.Post('Role', request);
  }
}
