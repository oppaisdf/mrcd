import { Component, computed, inject, input, model } from '@angular/core';
import { UiStickersComponent } from '../../../core/ui/stickers/ui-stickers.component';
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";
import { AssignedBaseEntityResponse } from '../reponses/Assigned-BaseEntity.response';
import { AlertService } from '../../alerts/services/alert.service';
import { BaseEntitiesService } from '../services/base-entities.service';

@Component({
  selector: 'shared-assign-base-entity',
  imports: [AccordeonComponent, UiStickersComponent],
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

  readonly loading = this._alert.loading;
  readonly stickers = computed(() =>
    this.items()
      .map(item => ({
        id: item.id,
        name: item.name,
        selected: item.hasAssociation
      }))
  );

  async assignAsync(
    id: string
  ) {
    const items = this.items();
    const item = items.find(i => i.id == id);
    if (!item) return;
    if (this._alert.loading()) return;
    let prefix = item.hasAssociation
      ? "des"
      : '';

    this._alert.confirm(
      `¿Deseas ${item.hasAssociation ? 'quitar' : 'asociar'} «${item.name}» ${item.hasAssociation ? 'del' : 'al'} confirmando?`,
      () => this.saveAsync(item, items, prefix)
    );
  }

  private async saveAsync(
    item: AssignedBaseEntityResponse,
    items: Array<AssignedBaseEntityResponse>,
    prefix: string
  ) {
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
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success(`Se ha ${prefix}asignado correctamente`);
    item.hasAssociation = !item.hasAssociation;
    this.items.set([...items]);
  }
}
