import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { PagedResult } from '../../../core/api/api.types';
import { LogResponse } from '../responses/log.response';

@Injectable()
export class LogService {
  private readonly _api = inject(ApiService);

  public toListAsync(
    page: number
  ) {
    var param: Record<string, any> = {
      page: page
    };
    return this._api.getAsync<PagedResult<LogResponse>>("/logs", param);
  }
}
