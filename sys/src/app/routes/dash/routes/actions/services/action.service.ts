import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { DefaultResponse } from '../../../models/Response';
import { DefaultRequest } from '../../../models/Request';

@Injectable()
export class ActionService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetAsync() {
    return await this._api.Get<DefaultResponse[]>('ActionLog');
  }

  public async AddAsync(
    request: DefaultRequest
  ) {
    return await this._api.Post('ActionLog', request);
  }

  public async UpdateAsync(
    id: number,
    request: DefaultRequest
  ) {
    return await this._api.Put(`ActionLog/${id}`, request);
  }
}
