import { Component, inject, signal } from '@angular/core';
import { PersonFormComponent } from "../form/person-form.component";
import { PersonVM } from '../vms/person.vm';
import { CreatePersonRequest } from '../requests/create-person.request';
import { FormBuilder, Validators } from '@angular/forms';
import { CreateSimpleParentRequest } from '../../parents/requests/create-simple-parent.request';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PersonService } from '../services/person.service';
import { PersonParentComponent } from "../parents/person-parent.component";

@Component({
  selector: 'person-create.page',
  imports: [
    PersonFormComponent,
    PersonParentComponent
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

  private readonly _parents = signal<Array<CreateSimpleParentRequest>>([]);
  get parents() { return this._parents(); }
  set parents(value) { this._parents.set(value); }

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
      parents: this._parents()
    };
    const response = await this._service.addAsync(request);
    this._alert.clear();
    if (!response.isSuccess)
      this._alert.error(response.message!);
    else this._alert.success("Se ha inscrito el confirmando correctamente");
  }
}
