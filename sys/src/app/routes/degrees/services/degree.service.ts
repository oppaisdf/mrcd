import { Injectable } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { DefaultResponse } from '../../../core/models/responses/Response';
import { DefaultRequest } from '../../../core/models/requests/Request';

@Injectable()
export class DegreeService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetAsync() {
    return await this._api.Get<DefaultResponse[]>('Degree');
  }

  public async AddAsync(
    request: DefaultRequest
  ) {
    return await this._api.Post('Degree', request);
  }

  public async UpdateAsync(
    id: number,
    request: DefaultRequest
  ) {
    return await this._api.Put(`Degree/${id}`, request);
  }
}
