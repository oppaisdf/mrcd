import { Injectable } from '@angular/core';
import { ParentRequest } from '../models/requests/person';
import { ApiService } from '../../../services/api.service';

@Injectable()
export class ParentService {
  constructor(
    private _api: ApiService
  ) { }

  public async UnassignAsync(
    personId: number,
    parentId: number
  ) {
    return await this._api.Delete(`Parent/${personId}/${parentId}`);
  }

  public async CreateAsync(
    id: number,
    request: ParentRequest
  ) {
    return await this._api.Post<{ id: number }>(`Parent/${id}`, request);
  }
}
