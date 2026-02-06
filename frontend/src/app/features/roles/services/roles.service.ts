import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { RoleResponse } from '../responses/Role.response';

@Injectable()
export class RolesService {
  private readonly _api = inject(ApiService);

  public toListAsync() {
    return this._api.getAsync<Array<RoleResponse>>('/role/permission');
  }

  public assignPermissionAsync(
    roleId: string,
    permissionId: string
  ) {
    return this._api.postAsync(`/permission/${permissionId}/role/${roleId}`, undefined);
  }

  public unassignPermissionAsync(
    roleId: string,
    permissionId: string
  ) {
    return this._api.delAsync(`/permission/${permissionId}/role/${roleId}`);
  }
}
