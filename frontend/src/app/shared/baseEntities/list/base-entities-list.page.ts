import { Component, effect, inject, input, signal } from '@angular/core';
import { BaseEntitiesService } from '../services/base-entities.service';
import { AlertService } from '../../alerts/services/alert.service';
import { BaseEntityResponse } from '../reponses/BaseEntity.response';

@Component({
  selector: 'shared-be-list',
  imports: [],
  templateUrl: './base-entities-list.page.html',
  styleUrl: './base-entities-list.page.scss',
})
export class BaseEntitiesListPage {
  private readonly _service = inject(BaseEntitiesService);
  private readonly _alert = inject(AlertService);
  title = input.required<string>();
  endpoint = input.required<string>();
  readonly items = signal<Array<BaseEntityResponse>>([]);

  private _ = effect(() => this.loadAsync());
  private async loadAsync() {
    const response = await this._service.toListAsync(this.endpoint());
    if (!response.isSuccess)
      this._alert.error(response.message!);
    else this.items.set(response.data ?? []);
  }

  protected async deleteAsync(
    id: string
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = await this._service.deleteAsync(this.endpoint(), id);

    this._alert.clear();
    if (response.isSuccess)
      this._alert.success("Se ha eliminado el registro correctamente");
    else if (response.message) this._alert.error(response.message);
  }
}
