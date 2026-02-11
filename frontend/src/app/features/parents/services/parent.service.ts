import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreateParentRequest } from '../requests/create-parent.request';

@Injectable()
export class ParentService {
  private readonly _api = inject(ApiService);

  public createAsync(
    request: CreateParentRequest
  ) {
    return this._api.postAsync<CreateParentRequest, string>('/parent', request);
  }
}
