import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { BaseEntityRequest } from '../requests/BaseEntity.request';
import { BaseEntityResponse } from '../reponses/BaseEntity.response';

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
    return this._api.getAsync<Array<BaseEntityResponse>>(endpoint);
  }

  public assignAsync(
    endpoint: string,
    personId: string,
    entityId: string
  ) {
    return this._api.postAsync(`${endpoint}/${entityId}/person/${personId}`, undefined);
  }

  public unassignAsync(
    endpoint: string,
    personId: string,
    entityId: string
  ) {
    return this._api.delAsync(`${endpoint}/${entityId}/person/${personId}`);
  }
}
