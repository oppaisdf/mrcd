import { Component, effect, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UserVM } from '../vms/UserVM';

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
  user = input.required<UserVM>();
  submit = output<UserVM>();
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
    this.submit.emit(this.form.getRawValue() as UserVM);
  }
}
