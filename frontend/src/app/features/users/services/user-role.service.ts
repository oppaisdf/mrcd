import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';

@Injectable()
export class UserRoleService {
  private readonly _api = inject(ApiService);

  public assignRoleAsync(
    userId: string,
    roleId: string
  ) {
    return this._api.postAsync(`/user/${userId}/role/${roleId}`, undefined);
  }

  public unassignRoleAsync(
    userId: string,
    roleId: string
  ) {
    return this._api.delAsync(`/user/${userId}/role/${roleId}`);
  }
}
