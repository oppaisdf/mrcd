import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';

@Injectable()
export class AttendanceService {
  constructor(
    private _api: ApiService
  ) { }

  public async ScanAsync(
    qr: string
  ) {
    return await this._api.Post(`Attendance/${qr}`, undefined);
  }
}
