import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreatePersonRequest } from '../requests/create-person.request';

@Injectable()
export class PersonService {
  private readonly _api = inject(ApiService);

  public addAsync(
    request: CreatePersonRequest
  ) {
    return this._api.postAsync<CreatePersonRequest, string>('/person', request);
  }
}
