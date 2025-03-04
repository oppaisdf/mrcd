import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { ParentResponse } from '../models/response';
import { ParentFilter } from '../models/requests';
import { ParentRequest } from '../../people/models/requests/person';

@Injectable()
export class ParentService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetAsync(
    filters: ParentFilter
  ) {
    return await this._api.Get<ParentResponse[]>('Parent', filters);
  }

  public async GetByIdAsync(
    id: number
  ) {
    return await this._api.Get<ParentResponse>(`Parent/${id}`);
  }

  public async CreateAsync(
    request: ParentRequest
  ) {
    return await this._api.Post<{ id: number }>('Parent', request);
  }

  public async UpdateAsync(
    id: number,
    request: ParentRequest
  ) {
    return await this._api.Patch(`Parent/${id}`, request);
  }

  public async UnassignAsync(
    personId: number,
    parentId: number
  ) {
    return await this._api.Delete(`Parent/${personId}/${parentId}`);
  }

  public async AssignAsync(
    parentId: number,
    personId: number,
    isParent: boolean
  ) {
    return await this._api.Post(`Parent/${parentId}/${personId}`, isParent);
  }

  public async DeleteAsync(
    id: number
  ) {
    return this._api.Delete(`Parent/${id}`);
  }

  public async GetAlertAsync(
    alert: number
  ) {
    return await this._api.Get<ParentResponse[]>(`Alert/${alert}/Full`);
  }
}
