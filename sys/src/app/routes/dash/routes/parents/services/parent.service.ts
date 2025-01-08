import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { ParentResponse } from '../models/response';
import { ParentFilter } from '../models/requests';

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
}
