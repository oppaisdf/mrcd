import { Component, inject, OnInit, signal } from '@angular/core';
import { PersonFormComponent } from "../form/person-form.component";
import { PersonVM } from '../vms/person.vm';
import { CreatePersonRequest } from '../requests/create-person.request';
import { FormBuilder, Validators } from '@angular/forms';
import { CreateSimpleParentRequest } from '../../parents/requests/create-simple-parent.request';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PersonService } from '../services/person.service';
import { AddSimpleParentComponent } from "../../parents/simple-add/add-simple-parent.component";
import { BaseEntitiesService } from '../../../shared/baseEntities/services/base-entities.service';
import { AssignedBaseEntityResponse } from '../../../shared/baseEntities/reponses/Assigned-BaseEntity.response';

@Component({
  selector: 'person-create.page',
  imports: [
    PersonFormComponent,
    AddSimpleParentComponent
  ],
  templateUrl: './person-create.page.html',
  styleUrl: './person-create.page.scss',
})
export class PersonCreatePage implements OnInit {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(PersonService);
  private readonly _sacraments = inject(BaseEntitiesService);
  readonly form = this._form.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(80)]],
    isMasculine: [false],
    phone: ['']
  });

  private readonly _parents = signal<Array<CreateSimpleParentRequest>>([]);
  readonly sacraments = signal<Array<AssignedBaseEntityResponse>>([]);
  get parents() { return this._parents(); }
  set parents(value) { this._parents.set(value); }

  async ngOnInit() {
    const response = await this._sacraments.toListAsync("/sacrament");
    if (response.isSuccess)
      this.sacraments.set(response.data?.map(s => ({
        id: s.id,
        name: s.name,
        hasAssociation: false
      })) ?? []);
  }

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
      parents: this._parents(),
      sacraments: this.sacraments()
        .filter(s => s.hasAssociation)
        .map(s => s.id)
    };
    const response = await this._service.addAsync(request);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success("Se ha inscrito el confirmando correctamente");
    this._parents.set([]);
  }

  assignSacrament(
    sacramentId: string
  ) {
    const sacraments = this.sacraments();
    const sacrament = sacraments.find(s => s.id === sacramentId);
    if (!sacrament) return;
    sacrament.hasAssociation = !sacrament.hasAssociation;
    this.sacraments.set(sacraments);
  }
}
