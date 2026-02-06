import { Component, effect, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UpdateUserRequest } from '../requests/update-user.request';

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
  readonly form = this._form.nonNullable.group({
    username: ['', [Validators.required, Validators.maxLength(10)]],
    password: ['', [
      Validators.required,
      Validators.pattern('^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{6,}$')
    ]],
    isActive: [true, Validators.required]
  });

  mode = input.required<'Crear' | 'Editar'>();
  user = input.required<UpdateUserRequest>();
  submit = output<UpdateUserRequest>();
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
      this.form.patchValue({
        username: user.username,
        password: user.password ?? '',
        isActive: user.isActive
      }, { emitEvent: false });
      if (user.isActive ?? true) {
        this.form.enable();
        return;
      }
      this.form.disable();
      this.form.controls.isActive.enable();
    });
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.form.pending) return;
    this.submit.emit(this.form.getRawValue() as UpdateUserRequest);
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
}
