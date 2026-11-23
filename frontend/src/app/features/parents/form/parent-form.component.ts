import { Component, inject, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';
import { CreateParentRequest } from '../requests/create-parent.request';
import { UiSwitchComponent } from '../../../core/ui/switch/ui-switch.component';

@Component({
  selector: 'parents-form',
  imports: [
    ReactiveFormsModule,
    UiInputComponent,
    UiSwitchComponent
  ],
  templateUrl: './parent-form.component.html',
  styleUrl: './parent-form.component.scss',
})
export class ParentFormComponent {
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(80)]],
    isMasculine: [false],
    phone: [''],
    isParent: [true]
  });

  hideIsParent = input.required<boolean>();
  personId = input<string>();
  parent = output<CreateParentRequest>();

  hint(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    if (!control) return null;
    if (!control.touched || control.valid) return null;
    const error = ({
      'name': 'El nombre es requerido y no puede exceder los 80 caracteres',
      'isMasculine': null,
      'isParent': null,
      'phone': 'El teléfono debe ser numérico'
    } as const)[controlName];
    return error;
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.form.pending) return;
    const raw = this.form.getRawValue();
    const request: CreateParentRequest = {
      parentName: raw.name,
      isMasculine: raw.isMasculine,
      isParent: raw.isParent,
      phone: raw.phone,
      personId: this.personId()
    };
    this.parent.emit(request);
    this.form.reset();
  }
}
