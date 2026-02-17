import { Component, computed, effect, inject, input, linkedSignal, signal } from '@angular/core';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";
import { AssignedParentResponse } from '../responses/assigned-parent.response';
import { ParentFormComponent } from "../form/parent-form.component";
import { CreateParentRequest } from '../requests/create-parent.request';
import { ParentService } from '../services/parent.service';

@Component({
  selector: 'parents-assign',
  imports: [AccordeonComponent, ParentFormComponent],
  templateUrl: './assign-parent.component.html',
  styleUrl: './assign-parent.component.scss',
})
export class AssignParentComponent {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(ParentService);
  typeParent = input.required<boolean>();
  rawParents = input.required<Array<AssignedParentResponse>>();
  readonly parents = linkedSignal(() => this.rawParents());
  personId = input.required<string>();

  fullParents = computed(() => this.parents().length === 2);

  constructor() {
    effect(() => {
      const raw = this.rawParents();
      this.parents.set(raw);
    });
  }

  async addAsync(
    request: CreateParentRequest
  ) {
    if (this._alert.loading()) return;
    request.personId = this.personId();
    request.isParent = this.typeParent();
    this._alert.startLoading();

    const response = await this._service.createAsync(request);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success(`El ${request.isParent ? "padre" : "padrino"} ha sido agregado correctamente`);
    const parent: AssignedParentResponse = {
      id: response.data!,
      name: request.parentName,
      isMasculine: request.isMasculine,
      phone: request.phone
    };
    this.parents.update(parents => [...parents, parent]);
  }

  async delAsync(
    parentId: string
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const isParent = this.typeParent();
    const response = await this._service.unassignAsync(parentId, this.personId(), isParent);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success(`Se ha desasignado el padr${isParent ? 'e' : 'ino'} correctamente`);
    this.parents.update(parents => parents.filter(p => p.id !== parentId));
  }
}
