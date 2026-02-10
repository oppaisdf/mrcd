import { Component, inject, input, model } from '@angular/core';
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";
import { AssignedBaseEntityResponse } from '../reponses/Assigned-BaseEntity.response';
import { AlertService } from '../../alerts/services/alert.service';
import { BaseEntitiesService } from '../services/base-entities.service';

@Component({
  selector: 'shared-assign-base-entity',
  imports: [AccordeonComponent],
  templateUrl: './assign-base-entity.component.html',
  styleUrl: './assign-base-entity.component.scss',
})
export class AssignBaseEntityComponent {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(BaseEntitiesService);
  title = input.required<string>();
  endpoint = input.required<string>();
  personId = input.required<string>();
  items = model.required<Array<AssignedBaseEntityResponse>>();

  async assignAsync(
    id: string
  ) {
    const items = this.items();
    const item = items.find(i => i.id == id);
    if (!item) return;
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = item.hasAssociation
      ? await this._service.unassignAsync(
        this.endpoint(),
        this.personId(),
        item.id
      ) : await this._service.assignAsync(
        this.endpoint(),
        this.personId(),
        item.id
      );
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success(`Se ha ${item.hasAssociation ? "des" : ''}asignado correctamente`);
    item.hasAssociation = !item.hasAssociation;
    this.items.set([...items]);
  }
}
