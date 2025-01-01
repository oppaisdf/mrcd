import { Injectable } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { DefaultRequest } from '../../routes/dash/models/Request';

@Injectable()
export class UpdateService {
  constructor(
    private _api: ApiService
  ) { }

  public async UpdateAsyc(
    id: number,
    request: DefaultRequest,
    endpoint: string
  ) {
    return await this._api.Put(`${endpoint}/${id}`, request);
  }
}
