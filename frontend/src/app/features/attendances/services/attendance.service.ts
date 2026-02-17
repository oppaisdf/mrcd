import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { AttendanceRequest } from '../requests/attendance.request';

@Injectable()
export class AttendanceService {
  private readonly _api = inject(ApiService);

  addAsync(
    request: AttendanceRequest
  ) {
    return this._api.postAsync('/attendance', request);
  }
}
