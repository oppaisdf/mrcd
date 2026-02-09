import { Component, effect, inject, input, OnInit, output, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { PersonVM } from '../vms/person.vm';
import { PersonResponse } from '../responses/person.response';
import { dateRangeValidator } from '../../../core/utils/date.validator';
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { BaseEntitiesService } from '../../../shared/baseEntities/services/base-entities.service';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";

@Component({
  selector: 'person-form',
  imports: [
    ReactiveFormsModule,
    UiInputComponent,
    UiSelectComponent
  ],
  templateUrl: './person-form.component.html',
  styleUrl: './person-form.component.scss',
})
export class PersonFormComponent implements OnInit {
  private readonly _form = inject(FormBuilder);
  private readonly _service = inject(BaseEntitiesService);

  readonly degrees = signal<Array<SelectItem<string>>>([]);
  readonly form = this._form.nonNullable.group({
    name: [''],
    isActive: [false],
    isMasculine: [false],
    isSunday: [true],
    parish: [''],
    address: [''],
    phone: [''],
    degreeId: [''],
    dob: [new Date()]
  });

  mode = input.required<'Inscribir' | 'Actualizar'>();
  person = input<PersonResponse>();
  formSubmit = output<PersonVM>();

  constructor() {
    effect(() => {
      const isCreate = this.mode() === 'Inscribir';
      this.addValidators(isCreate);
      if (isCreate) return;
      const person = this.person();
      if (!person) return;
      this.form.reset({
        name: person.name,
        isActive: person.isActive,
        isMasculine: person.isMasculine,
        isSunday: person.isSunday,
        parish: person.parish,
        address: person.adress,
        phone: person.phone,
        degreeId: person.degreeId,
        dob: person.dob
      });
    });
  }

  async ngOnInit() {
    const response = await this._service.toListAsync("/degree");
    if (!response.isSuccess) return;
    const degrees = response.data?.map(d => ({
      label: d.name,
      value: d.id
    }));
    this.degrees.set(degrees ?? []);
  }

  private addValidators(
    isCreate: boolean
  ) {
    const controls = this.form.controls;
    controls.name.setValidators([Validators.maxLength(65)]);
    controls.address.setValidators([Validators.maxLength(100)]);
    controls.dob.setValidators([dateRangeValidator(15, 25)]);

    if (isCreate) {
      controls.name.addValidators(Validators.required);
      controls.address.addValidators(Validators.required);
      controls.phone.setValidators([Validators.required]);
      controls.dob.addValidators(Validators.required);
    }

    this.form.updateValueAndValidity({ emitEvent: false });
  }

  onSubmit() {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.form.pending) return;

    const form = this.form.getRawValue();
    const response: PersonVM = {
      name: form.name,
      isActive: form.isActive,
      isMasculine: form.isMasculine,
      isSunday: form.isSunday,
      parish: form.parish,
      address: form.address,
      phone: form.phone,
      degreeId: form.degreeId,
      dob: form.dob
    };
    this.formSubmit.emit(response);
  }

  dateLimit(
    isMax: boolean
  ) {
    const now = new Date();
    now.setFullYear(now.getFullYear() - (isMax ? 15 : 25));
    return now.toISOString().split('T')[0];
  }

  hint(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    const isCreateMode = this.mode() === 'Inscribir';
    if (!control) return 'Control not found :c';
    if (!control.touched || control.valid) return null;

    switch (controlName) {
      case 'name':
        if (isCreateMode) return 'El nombre es requerido y no puede exceder los 65 caracteres';
        else return 'El nombre no puede exceder los 65 caracteres';
      case 'address':
        if (isCreateMode) return 'La dirección es requerida y no puede exceder los 100 caracteres';
        else return 'La dirección no puede exceder los 100 caracteres';
      case 'phone':
        if (isCreateMode) return 'El número telefónico es requerido';
        break;
      case 'parish':
        if (!isCreateMode) return 'La parroquia de bautizo no puede exceder los 30 caracteres';
        break;
      case 'dob':
        if (isCreateMode) return 'La fecha de nacimiento es requerida y el confirmando debe tener entre 15-25 años';
        else return 'El confirmando debe tener entre 15-25 años';
    }
    return null;
  }

  arrayBoolean(
    type: 'states' | 'genders' | 'days'
  ) {
    const labelA = ({
      'states': 'Activo',
      'genders': 'Masculino',
      'days': 'Domingo'
    } as const)[type] ?? 'unknow';
    const labelB = ({
      'states': 'Inactivo',
      'genders': 'Femenino',
      'days': 'Sábado'
    } as const)[type] ?? 'unknow';
    return [{
      label: labelA,
      value: true
    }, {
      label: labelB,
      value: false
    }];
  }
}
