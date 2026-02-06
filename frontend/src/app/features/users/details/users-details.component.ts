import { Component, computed, effect, inject, model } from '@angular/core';
import { UserDTO } from '../dtos/UserDTO';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { UserRoleService } from '../services/user-role.service';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UsedRoleResponse } from '../../roles/responses/UsedRole.response';

@Component({
  selector: 'users-details',
  imports: [
    UiInputComponent,
    ReactiveFormsModule
  ],
  templateUrl: './users-details.component.html',
  styleUrl: './users-details.component.scss',
})
export class UsersDetailsComponent {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(UserRoleService);
  private readonly _form = inject(FormBuilder);

  user = model.required<UserDTO>();
  roles = computed(() => this.user().roles);
  isActive = computed(() => this.user().isActive);
  readonly form = this._form.nonNullable.group({
    username: ['', [Validators.required, Validators.maxLength(10)]],
    isActive: [false]
  });

  constructor() {
    effect(() => {
      const user = this.user();
      this.form.patchValue({
        username: user.username,
        isActive: user.isActive
      }, { emitEvent: false });
      if (user.isActive) this.form.controls.username.enable();
      else this.form.controls.username.disable();
    });
  }

  protected async assignAsync(
    role: UsedRoleResponse,
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
    const user = this.user();
    this.user.set({
      ...user,
      roles: user.roles.map(r =>
        r.id === role.id ? { ...r, hasRole: !r.hasRole } : r
      ),
    });
  }
}
