import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { RoleDTO } from '../dtos/RoleDTO';

@Injectable()
export class RolesService {
  private readonly _api = inject(ApiService);

  public toListAsync() {
    return this._api.getAsync<RoleDTO[]>('/role/permission');
  }
}
