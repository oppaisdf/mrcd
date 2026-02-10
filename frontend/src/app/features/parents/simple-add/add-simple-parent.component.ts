import { Component, inject, input, model } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateSimpleParentRequest } from '../requests/create-simple-parent.request';
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UiSelectComponent } from '../../../core/ui/select/ui-select.component';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';

@Component({
  selector: 'parents-add-simple',
  imports: [
    ReactiveFormsModule,
    UiSelectComponent,
    UiInputComponent
  ],
  templateUrl: './add-simple-parent.component.html',
  styleUrl: './add-simple-parent.component.scss',
})
export class AddSimpleParentComponent {
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(80)]],
    isMasculine: [false, Validators.required],
    phone: []
  });
  parents = model.required<Array<CreateSimpleParentRequest>>();
  title = input.required<string>();

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
