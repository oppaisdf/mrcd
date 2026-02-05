import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from "@angular/router";
import { RolesService } from '../services/roles.service';
import { RoleDTO } from '../dtos/RoleDTO';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { RolePermissionsComponent } from "../permissions.component/role-permissions.component";

@Component({
  selector: 'app-roles-list.page',
  imports: [RouterLink, RolePermissionsComponent],
  templateUrl: './roles-list.page.html',
  styleUrl: './roles-list.page.scss',
})
export class RolesListPage implements OnInit {
  private readonly _service = inject(RolesService);
  protected readonly roles = signal<Array<RoleDTO>>([]);
  private readonly _alert = inject(AlertService);
  private readonly _selectedRole = signal<RoleDTO | null>(null);

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
    role: RoleDTO | null
  ) { this._selectedRole.set(role); }

  protected selectRole(
    role: RoleDTO
  ) {
    this.selectedRole = role;
  }
}
