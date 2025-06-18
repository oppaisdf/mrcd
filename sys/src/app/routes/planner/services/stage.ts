import { Injectable } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { DefaultResponse } from '../../../core/models/responses/Response';
import { DefaultRequest } from '../../../core/models/requests/Request';

@Injectable()
export class StageService {
  constructor(
    private _api: ApiService
  ) { }

  public async StagesToListAsync() {
    return await this._api.Get<DefaultResponse[]>('Planner/Stage');
  }

  public async DeleteStageAsync(
    id: number
  ) {
    return await this._api.Delete(`Planner/Stage/${id}`);
  }

  public async AddStageAsync(
    request: DefaultRequest
  ) {
    return await this._api.Post('Planner/Stage', request);
  }
}
