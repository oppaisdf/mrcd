import { Injectable } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { DayResponse, MonthResponse, PlannerResponse, SimpleActivityResponse } from '../models/responses';
import { ActivityRequest, ActivityStageRequest } from '../models/requests';

@Injectable()
export class CalendarService {
  constructor(
    private _api: ApiService
  ) { }

  Months: MonthResponse[] = Array.from({ length: 12 }, (_, i) => {
    const now = new Date();
    const date = new Date(now.getFullYear(), i);
    return {
      name: date.toLocaleString('es-ES', { month: 'long' }),
      value: date.getMonth()
    };
  });

  public async GetAsync(
    month: number
  ) {
    return await this._api.Get<DayResponse[]>(`Planner/${month}`);
  }

  public async CreateActivityAsync(
    request: ActivityRequest
  ) {
    return await this._api.Post<{ id: number }>('Planner/Activity', request);
  }

  public async ActivityById(
    id: number
  ) {
    return await this._api.Get<PlannerResponse>(`Planner/Activity/${id}`);
  }

  public async NextActivityAsync() {
    return await this._api.Get<SimpleActivityResponse>('Planner/Activity/Next');
  }

  public async DeleteActivityAsync(
    id: number
  ) {
    return await this._api.Delete(`Planner/Activity/${id}`);
  }

  public async AddActivityToStageAsync(
    request: ActivityStageRequest
  ) {
    return await this._api.Post('Planner/ActivityStage', request);
  }

  public async DelStageToActivityAsync(
    activityId: number,
    stageId: number
  ) {
    return await this._api.Delete(`Planner/${activityId}/${stageId}`);
  }
}
