import { Component, computed, effect, inject, input, output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UserResponse } from '../responses/User.response';
import { UserVM } from '../vms/User.vm';
import { UsedRoleResponse } from '../../roles/responses/UsedRole.response';

@Component({
  selector: 'user-form',
  imports: [
    ReactiveFormsModule,
    UiInputComponent,
    UiSelectComponent
  ],
  templateUrl: './user-form.component.html',
  styleUrl: './user-form.component.scss',
})
export class UserFormComponent {
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.group({
    username: [''],
    password: [''],
    isActive: [true]
  });

  mode = input.required<'Crear' | 'Editar'>();
  user = input.required<UserResponse>();
  formSubmit = output<UserVM>();

  readonly roles = signal<Array<UsedRoleResponse>>([]);
  private readonly _roles: Array<string> = [];
  readonly states: Array<SelectItem<boolean>> = [
    {
      label: 'Activo',
      value: true
    }, {
      label: 'Inactivo',
      value: false
    }
  ];

  constructor() {
    effect(() => {
      const user = this.user();
      this.roles.set(user.roles);
      this.form.patchValue({
        username: user.username,
        password: '',
        isActive: user.isActive
      }, { emitEvent: false });
      this.addValidators();
    });
  }

  private addValidators() {
    const isCreate = this.mode() === 'Crear';

    const username = this.form.controls.username;
    const password = this.form.controls.password;

    username.setValidators([Validators.maxLength(10)]);
    password.setValidators([
      Validators.pattern('^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{6,}$'),
    ]);

    if (isCreate) {
      username.addValidators(Validators.required);
      password.addValidators(Validators.required);
    }

    this.form.updateValueAndValidity({ emitEvent: false });
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.form.pending) return;

    const form = this.form.getRawValue();
    const response: UserVM = {
      username: form.username,
      password: form.password
    };
    const mode = this.mode();
    if (mode === 'Editar')
      response.isActive = form.isActive!;
    else response.roles = this._roles;
    if (mode === 'Crear' && this._roles.length < 1) return;
    this.formSubmit.emit(response);
  }

  invalidField(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    if (!control) return null;
    if (!control.touched || control.valid) return null;
    switch (controlName) {
      case 'username':
        return `El nombre del usuario es requerido`;
      case 'password':
        return `La contraseña debe tener un número, una mayúscula, un carácter especial y su longitud debe ser mayor a cinco caracteres`;
    }
    return null;
  }

  get emptyRoles() { return this._roles.length < 1; }

  private indexRoleAdded = (roleId: string) => this._roles.indexOf(roleId);
  roleAdded = (roleId: string) => this.indexRoleAdded(roleId) !== -1;

  addRole(
    roleId: string
  ) {
    const index = this.indexRoleAdded(roleId);
    if (index !== -1)
      this._roles.splice(index, 1);
    else this._roles.push(roleId);
  }
}
