import { Component, computed, inject, model } from '@angular/core';
import { RoleDTO } from '../dtos/RoleDTO';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { RolesService } from '../services/roles.service';
import { PermissionResponse } from '../../permissions/reponses/Permission.response';

@Component({
  selector: 'role-permissions',
  imports: [],
  templateUrl: './role-permissions.component.html',
  styleUrl: './role-permissions.component.scss',
})
export class RolePermissionsComponent {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(RolesService);

  role = model.required<RoleDTO>();
  permissions = computed(() => this.role().permissions);

  protected async assignAsync(
    permission: PermissionResponse,
    isAssignation: boolean
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();
    const roleId = this.role().roleID;

    const response = isAssignation
      ? await this._service.assignPermissionAsync(roleId, permission.permissionID)
      : await this._service.unassignPermissionAsync(roleId, permission.permissionID);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message!);
      return;
    }
    const role = this.role();
    this.role.set({
      ...role,
      permissions: role.permissions.map(p =>
        p.permissionID === permission.permissionID ? {
          ...p,
          isUsed: !p.isUsed
        } : p
      )
    });
    this._alert.success(`Se ha ${isAssignation ? 'des' : ''}asignado el permiso al rol correctamente`);
  }
}
