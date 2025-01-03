import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { PersonRequest } from '../models/requests/person';
import { PersonFilterRequest, PersonFilterResponse, PersonResponse } from '../models/responses/person';

@Injectable()
export class PersonService {
  constructor(
    private _api: ApiService
  ) { }

  public async AddAsync(
    request: PersonRequest
  ) {
    return await this._api.Post<{ id: number }>('People', request);
  }

  public async UpdateAsync(
    id: number,
    request: PersonRequest
  ) {
    return await this._api.Patch(`People/${id}`, request);
  }

  public async GetFiltersAsync() {
    return await this._api.Get<PersonFilterResponse>('People/Filters');
  }

  public async GetAsync(
    filter: PersonFilterRequest
  ) {
    return await this._api.Get<PersonResponse[]>('People', filter);
  }

  public async GetByIdAsync(
    id: number
  ) {
    return await this._api.Get<PersonResponse>(`People/${id}`);
  }
}
