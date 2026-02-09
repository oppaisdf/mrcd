import { Component, inject, signal } from '@angular/core';
import { PersonFormComponent } from "../form/person-form.component";
import { PersonVM } from '../vms/person.vm';
import { CreatePersonRequest } from '../requests/create-person.request';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateSimpleParentRequest } from '../../parents/requests/create-simple-parent.request';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PersonService } from '../services/person.service';

@Component({
  selector: 'person-create.page',
  imports: [
    PersonFormComponent,
    ReactiveFormsModule,
    UiInputComponent,
    UiSelectComponent
  ],
  templateUrl: './person-create.page.html',
  styleUrl: './person-create.page.scss',
})
export class PersonCreatePage {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(PersonService);
  readonly form = this._form.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(80)]],
    isMasculine: [false],
    phone: ['']
  });

  readonly parents = signal<Array<CreateSimpleParentRequest>>([]);
  get fullParents() { return this.parents().length === 2; }
  readonly genders: Array<SelectItem<boolean>> = [
    {
      label: 'Masculino',
      value: true
    }, {
      label: 'Femenino',
      value: false
    }
  ];

  async createAsync(
    person: PersonVM
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const request: CreatePersonRequest = {
      name: person.name ?? '',
      isMasculine: person.isMasculine ?? false,
      isSunday: person.isSunday ?? false,
      dob: person.dob ?? new Date(),
      address: person.address ?? '',
      phone: person.phone ?? '',
      degreeId: person.degreeId ?? '',
      parents: this.parents()
    };
    const response = await this._service.addAsync(request);
    this._alert.clear();
    if (!response.isSuccess)
      this._alert.error(response.message!);
    else this._alert.success("Se ha inscrito el confirmando correctamente");
  }

  addParent() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
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

  delParent(
    parent: CreateSimpleParentRequest
  ) {
    const parents = this.parents();
    const index = parents.indexOf(parent);
    parents.splice(index, 1);
    this.parents.set(parents);
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
