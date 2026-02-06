import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { BaseEntityDTO } from '../dtos/BaseEntityDTO';
import { BaseEntityRequest } from '../requests/BaseEntity.request';

@Injectable()
export class BaseEntitiesService {
  private readonly _api = inject(ApiService);

  public createAsync(
    endpoint: string,
    request: BaseEntityRequest
  ) {
    return this._api.postAsync<BaseEntityRequest, string>(endpoint, request);
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
