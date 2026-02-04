import { Component, effect, inject, signal } from '@angular/core';
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
export class RolesListPage {
  private readonly _service = inject(RolesService);
  protected readonly roles = signal<Array<RoleDTO>>([]);
  private readonly _alert = inject(AlertService);
  readonly selectedRole = signal<RoleDTO | undefined>(undefined);

  private async loadAsync() {
    const response = await this._service.toListAsync();
    if (!response.isSuccess) {
      this._alert.error(response.message!);
      return;
    }
    this.roles.set(response.data ?? []);
  }
  private _ = effect(() => this.loadAsync());

  selectRole(
    role: RoleDTO
  ) {
    this.selectedRole.set(role);
  }
}
