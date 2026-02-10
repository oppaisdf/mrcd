import { Component, inject, model } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateSimpleParentRequest } from '../../parents/requests/create-simple-parent.request';
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';
import { UiSelectComponent } from '../../../core/ui/select/ui-select.component';

@Component({
  selector: 'people-parent',
  imports: [
    ReactiveFormsModule,
    UiInputComponent,
    UiSelectComponent
  ],
  templateUrl: './person-parent.component.html',
  styleUrl: './person-parent.component.scss',
})
export class PersonParentComponent {
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(80)]],
    isMasculine: [false, Validators.required],
    phone: []
  });
  parents = model.required<Array<CreateSimpleParentRequest>>();

  get fullParents() { return this.parents().length === 2; }
  readonly genders: Array<SelectItem<boolean>> = [{
    label: 'Masculino',
    value: true
  }, {
    label: 'Femenino',
    value: false
  }];

  removeParent(
    parent: CreateSimpleParentRequest
  ) {
    const parents = this.parents();
    const index = parents.indexOf(parent);
    parents.splice(index, 1);
    this.parents.set(parents);
  }

  addParent() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    if (this.fullParents) return;

    const parents = this.parents();
    const form = this.form.getRawValue();
    const parent: CreateSimpleParentRequest = {
      name: form.name,
      isMasculine: form.isMasculine,
      phone: form.phone
    };
    parents.push(parent);
    this.parents.set(parents);
    this.form.reset();
  }

  hint(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    if (!control) return `${controlName} not found`;
    if (!control.touched || control.valid) return null;
    const error = ({
      'name': 'El nombre del padre es requerido y no puede superar los 80 caracteres',
      'isMasculine': null,
      'phone': null
    } as const)[controlName] ?? null;
    return error;
  }
}
