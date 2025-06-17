import { Injectable } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { DayResponse, MonthResponse } from '../models/responses';
import { ActivityRequest } from '../models/requests';

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
}
