import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PersonService } from '../services/person.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PersonFormComponent } from "../form/person-form.component";
import { PersonResponse } from '../responses/person.response';
import { PersonVM } from '../vms/person.vm';
import { UpdatePersonRequest } from '../requests/update-person.request';
import { AssignedBaseEntityResponse } from '../../../shared/baseEntities/reponses/Assigned-BaseEntity.response';
import { AssignBaseEntityComponent } from "../../../shared/baseEntities/assign/assign-base-entity.component";
import { AssignedParentResponse } from '../../parents/responses/assigned-parent.response';

@Component({
  selector: 'app-people-details.page',
  imports: [PersonFormComponent, AssignBaseEntityComponent],
  templateUrl: './people-details.page.html',
  styleUrl: './people-details.page.scss',
})
export class PeopleDetailsPage implements OnInit {
  private readonly _me = inject(ActivatedRoute);
  private readonly _service = inject(PersonService);
  private readonly _alert = inject(AlertService);
  readonly id: string;
  readonly person = signal<PersonResponse>({
    name: '',
    isActive: false,
    isMasculine: false,
    isSunday: false,
    dob: new Date(),
    degreeId: ''
  });
  readonly documents = signal<Array<AssignedBaseEntityResponse>>([]);
  readonly sacraments = signal<Array<AssignedBaseEntityResponse>>([]);
  readonly charges = signal<Array<AssignedBaseEntityResponse>>([]);
  readonly parents = signal<Array<AssignedParentResponse>>([]);
  readonly godparents = signal<Array<AssignedParentResponse>>([]);

  constructor() {
    this.id = this._me.snapshot.paramMap.get("id")!;
  }

  async ngOnInit() {
    this._alert.startLoading();
    const response = await this._service.getByIdAsync(this.id);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    if (!response.data) return;
    const person: PersonResponse = {
      name: response.data.name,
      isActive: response.data.isActive,
      isMasculine: response.data.isMasculine,
      isSunday: response.data.isSunday,
      dob: response.data.dob,
      degreeId: response.data.degreeId,
      address: response.data.address,
      phone: response.data.phone,
      parish: response.data.parish
    };
    this.person.set(person);
    this.documents.set(response.data.documents);
    this.sacraments.set(response.data.sacraments);
    this.charges.set(response.data.charges);
    this.parents.set(response.data.parents);
    this.godparents.set(response.data.godparents);
  }

  async updateAsync(
    person: PersonVM
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const current = this.person();
    const request: UpdatePersonRequest = {
      name: person.name !== null && person.name !== current.name ? person.name : undefined,
      dob: person.dob !== null && person.dob !== current.dob ? person.dob : undefined,
      isActive: person.isActive !== null && person.isActive !== current.isActive ? person.isActive : undefined,
      isSunday: person.isSunday !== null && person.isSunday !== current.isSunday ? person.isSunday : undefined,
      parish: person.parish !== null && person.parish !== current.parish ? person.parish : undefined,
      address: person.address !== null && person.address !== current.address ? person.address : undefined,
      phone: person.phone !== null && person.phone !== current.phone ? person.phone : undefined
    };
    const response = await this._service.updateAsync(this.id, request);
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success("Se ha actualizado al confirmando correctamente");
    const newPerson = this.person();
    if (request.name !== undefined) newPerson.name = request.name;
    if (request.isActive !== undefined) newPerson.isActive = request.isActive;
    if (request.isSunday !== undefined) newPerson.isSunday = request.isSunday;
    if (request.dob !== undefined) newPerson.dob = request.dob;
    if (request.address !== undefined) newPerson.address = request.address;
    if (request.phone !== undefined) newPerson.phone = request.phone;
    if (request.parish !== undefined) newPerson.parish = request.parish;
    this.person.set(newPerson);
  }
}
