import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreateParentRequest } from '../requests/create-parent.request';
import { PagedResult } from '../../../core/api/api.types';
import { AssignedParentResponse } from '../responses/assigned-parent.response';

@Injectable()
export class ParentService {
  private readonly _api = inject(ApiService);

  public createAsync(
    request: CreateParentRequest
  ) {
    return this._api.postAsync<CreateParentRequest, string>('/parent', request);
  }

  public toListAsync(
    page: number,
    parentName: string | null
  ) {
    const params: Record<string, any> = {
      page: page,
      parentName: parentName
    };
    return this._api.getAsync<PagedResult<AssignedParentResponse>>('/parent', params);
  }
}
