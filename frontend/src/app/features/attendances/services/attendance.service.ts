import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { AttendanceRequest } from '../requests/attendance.request';
import { AttendanceResponse } from '../responses/attendance.response';

@Injectable()
export class AttendanceService {
  private readonly _api = inject(ApiService);

  addAsync(
    request: AttendanceRequest
  ) {
    return this._api.postAsync('/attendance', request);
  }

  delAsync(
    personId: string,
    date: Date
  ) {
    const dateString = date.toISOString().split('T')[0];
    return this._api.delAsync(`/attendance/${personId}/date/${dateString}`);
  }

  toListAsync(
    date: Date,
    filteredOnlyByYear?: boolean,
    isSunday?: boolean,
    isMasculine?: boolean,
    personName?: string
  ) {
    const params: Record<string, any> = {
      date: date,
      filteredOnlyByYear: filteredOnlyByYear,
      isSunday: isSunday,
      isMasculine: isMasculine,
      personName: personName
    }
    return this._api.getAsync<Array<AttendanceResponse>>('/attendance', params);
  }
}
