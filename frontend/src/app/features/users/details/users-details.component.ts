import { Component, computed, effect, inject, model, signal } from '@angular/core';
import { UserResponse } from '../responses/User.response';
import { UserFormComponent } from "../form/user-form.component";
import { UserVM } from '../vms/User.vm';
import { UsedRoleResponse } from '../../roles/responses/UsedRole.response';
import { UserService } from '../services/user.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { UpdateUserRequest } from '../requests/update-user.request';
import { UserRoleService } from '../services/user-role.service';

@Component({
  selector: 'users-details',
  imports: [UserFormComponent],
  templateUrl: './users-details.component.html',
  styleUrl: './users-details.component.scss',
})
export class UsersDetailsComponent {
  private readonly _userService = inject(UserService);
  private readonly _userRoleService = inject(UserRoleService);
  private readonly _alert = inject(AlertService);

  user = model.required<UserResponse>();
  readonly roles = signal<Array<UsedRoleResponse>>([]);
  userVM = computed(() => {
    const user = this.user();
    const userVM: UserVM = {
      username: user.username,
      password: null,
      isActive: user.isActive
    };
    return userVM;
  });

  constructor() {
    effect(() => {
      const user = this.user();
      this.roles.set(user.roles);
    });
  }

  async updateAsync(
    user: UserVM
  ) {
    if (this._alert.loading()) return;

    this._alert.startLoading();

    const currentUser = this.user();
    const request: UpdateUserRequest = {};

    if (
      user.isActive !== undefined &&
      currentUser.isActive !== user.isActive
    ) request.isActive = user.isActive;

    if (
      user.username !== null &&
      currentUser.username !== user.username
    ) request.username = user.username;

    if (user.password !== null)
      request.password = user.password;

    const response = await this._userService.updateAsync(
      currentUser.id,
      request
    );

    this._alert.clear();

    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success('Se ha actualizado el usuario exitosamente');

    if (request.isActive !== undefined)
      currentUser.isActive = request.isActive;

    if (request.username !== undefined)
      currentUser.username = request.username;

    this.user.set(currentUser);
  }

  async assignRoleAsync(
    roleId: string
  ) {
    if (this._alert.loading()) return;
    const user = this.user();
    const roles = this.roles();
    const role = roles.find(r => r.id === roleId);
    if (!role) return;

    this._alert.startLoading();
    const response = role.hasRole
      ? await this._userRoleService.unassignRoleAsync(user.id, roleId)
      : await this._userRoleService.assignRoleAsync(user.id, roleId);
    this._alert.clear();

    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success(`Se ha ${role.hasRole ? 'des' : ''}asignado el rol al usuario exitosamente`);
    const newRoles = roles.map(r => r.id === roleId ? { ...r, hasRole: !r.hasRole } : r);
    this.roles.set(newRoles);
  }
}
