import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { DefaultResponse } from '../../../models/Response';
import { DefaultRequest } from '../../../models/Request';

@Injectable()
export class DocService {
  constructor(
    private _service: ApiService
  ) { }

  public async GetAsync() {
    return await this._service.Get<DefaultResponse[]>('Document');
  }

  public async AddAsync(
    request: DefaultRequest
  ) {
    return await this._service.Post('Document', request);
  }

  public async UpdateAsync(
    id: number,
    request: DefaultRequest
  ) {
    return await this._service.Put(`Document/${id}`, request);
  }
}
