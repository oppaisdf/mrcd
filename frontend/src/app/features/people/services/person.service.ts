import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreatePersonRequest } from '../requests/create-person.request';
import { PagedResult } from '../../../core/api/api.types';
import { SimplePersonResponse } from '../responses/simple-person.response';

@Injectable()
export class PersonService {
  private readonly _api = inject(ApiService);

  public addAsync(
    request: CreatePersonRequest
  ) {
    return this._api.postAsync<CreatePersonRequest, string>('/person', request);
  }

  public toListAsync(
    isActive: boolean,
    page: number,
    name?: string,
    isMasculine?: boolean
  ) {
    const params: Record<string, any> = {
      page: page,
      isActive: isActive,
      name: name,
      isMasculine: isMasculine
    };
    return this._api.getAsync<PagedResult<SimplePersonResponse>>('/person', params);
  }
}
