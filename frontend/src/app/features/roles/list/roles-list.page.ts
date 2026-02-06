import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from "@angular/router";
import { RolesService } from '../services/roles.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { RolePermissionsComponent } from "../permissions.component/role-permissions.component";
import { RoleResponse } from '../responses/Role.response';

@Component({
  selector: 'app-roles-list.page',
  imports: [RouterLink, RolePermissionsComponent],
  templateUrl: './roles-list.page.html',
  styleUrl: './roles-list.page.scss',
})
export class RolesListPage implements OnInit {
  private readonly _service = inject(RolesService);
  protected readonly roles = signal<Array<RoleResponse>>([]);
  private readonly _alert = inject(AlertService);
  private readonly _selectedRole = signal<RoleResponse | null>(null);

  async ngOnInit() {
    const response = await this._service.toListAsync();
    if (!response.isSuccess) {
      this._alert.error(response.message!);
      return;
    }
    this.roles.set(response.data ?? []);
  }

  get selectedRole() { return this._selectedRole(); }
  set selectedRole(
    role: RoleResponse | null
  ) { this._selectedRole.set(role); }

  protected selectRole(
    role: RoleResponse
  ) {
    this.selectedRole = role;
  }
}
