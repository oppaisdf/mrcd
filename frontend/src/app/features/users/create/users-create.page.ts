import { Component, inject, OnInit, signal } from '@angular/core';
import { UserFormComponent } from "../form/user-form.component";
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { CreateUserRequest } from '../requests/create-user.request';
import { UserVM } from '../vms/User.vm';
import { RolesService } from '../../roles/services/roles.service';
import { UsedRoleResponse } from '../../roles/responses/UsedRole.response';

@Component({
  selector: 'app-users-create.page',
  imports: [UserFormComponent],
  templateUrl: './users-create.page.html',
  styleUrl: './users-create.page.scss',
})
export class UsersCreatePage implements OnInit {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(UserService);
  private readonly _rolesService = inject(RolesService);
  private readonly _router = inject(Router);

  private readonly _roles: Array<string> = [];
  readonly roles = signal<Array<UsedRoleResponse>>([]);
  readonly user = signal<UserVM>({
    username: null,
    password: null
  });

  async ngOnInit() {
    const response = await this._rolesService.toListAsync();
    if (!response.isSuccess) {
      if (response.message)
        this._alert.error(response.message);
      return;
    }
    const roles = (response.data ?? [])
      .map(r => ({
        id: r.roleID,
        roleName: r.roleName,
        hasRole: false
      }));
    this.roles.set(roles);
  }

  async createAsync(
    rawUser: UserVM
  ) {
    if (this._alert.loading()) return;
    if (this._roles.length < 1) return;
    this._alert.startLoading();

    const request: CreateUserRequest = {
      username: rawUser.username ?? '',
      password: rawUser.password ?? '',
      roles: this._roles
    };
    const response = await this._service.createAsync(request);
    this._alert.clear();
    if (!response.isSuccess) {
      if (response.message)
        this._alert.error(response.message);
      return;
    }
    this._alert.success('El usuario ha sido creado exitosamente');
    this._router.navigateByUrl('/users');
  }

  addRole(
    roleId: string
  ) {
    const usedRoles = this.roles();
    const usedRole = usedRoles.find(r => r.id === roleId);
    const roleIndex = this._roles.indexOf(roleId);
    if (!usedRole) return;
    if (!usedRole.hasRole && roleIndex === -1)
      this._roles.push(roleId);
    if (usedRole.hasRole && roleIndex !== -1)
      this._roles.splice(roleIndex, 1);
    usedRole.hasRole = !usedRole.hasRole;
    this.roles.set(usedRoles);
  }
}
