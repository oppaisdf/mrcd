import { Component, computed, inject, input, output } from '@angular/core';
import { UserDTO } from '../dtos/UserDTO';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { UserRoleService } from '../services/user-role.service';
import { UserRoleDTO } from '../../roles/dtos/UserRoleDTO';

@Component({
  selector: 'users-details',
  imports: [UiInputComponent],
  templateUrl: './users-details.component.html',
  styleUrl: './users-details.component.scss',
})
export class UsersDetailsComponent {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(UserRoleService);
  user = input.required<UserDTO>();
  updated = output<void>();

  roles = computed(() => this.user().roles);
  isActive = computed(() => this.user().isActive);

  protected async assignAsync(
    role: UserRoleDTO,
    isAssignation: boolean
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const userId = this.user().id;
    const response = isAssignation
      ? await this._service.assignRoleAsync(userId, role.id)
      : await this._service.unassignRoleAsync(userId, role.id);

    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message!);
      return;
    }
    this._alert.success(`Se ha ${isAssignation ? "des" : ""}asignado el rol al usuario correctamente`);
    this.updated.emit();
  }
}
