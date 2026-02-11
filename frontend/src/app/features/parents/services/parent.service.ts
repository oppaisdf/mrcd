import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreateparentRequest } from '../requests/create-parent.request';

@Injectable()
export class ParentService {
  private readonly _api = inject(ApiService);

  public createAsync(
    request: CreateparentRequest
  ) {
    return this._api.postAsync<CreateparentRequest, string>('/parent', request);
  }
}
