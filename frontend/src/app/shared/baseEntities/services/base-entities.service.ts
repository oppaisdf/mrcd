import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { BaseEntityCommand } from '../dtos/BaseEntityCommand';
import { BaseEntityDTO } from '../dtos/BaseEntityDTO';

@Injectable()
export class BaseEntitiesService {
  private readonly _api = inject(ApiService);

  public createAsync(
    endpoint: string,
    request: BaseEntityCommand
  ) {
    return this._api.postAsync<BaseEntityCommand, string>(endpoint, request);
  }

  public deleteAsync(
    endpoint: string,
    id: string
  ) {
    return this._api.delAsync(`${endpoint}/${id}`);
  }

  public toListAsync(
    endpoint: string
  ) {
    return this._api.getAsync<BaseEntityDTO[]>(endpoint);
  }
}
