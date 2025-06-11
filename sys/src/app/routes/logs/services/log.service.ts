import { Injectable } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { LogFilters } from '../models/request/logs';
import { FilterLogResponse, LogResponse } from '../models/responses/logs';

@Injectable()
export class LogService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetAsync(
    filters: LogFilters | undefined
  ) {
    return await this._api.Get<LogResponse[]>('Logs', filters);
  }

  public async GetFiltersAsync() {
    return await this._api.Get<FilterLogResponse>('Logs/Filters');
  }
}
