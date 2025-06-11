import { Injectable } from '@angular/core';
import { QRResponse } from '../../prints/responses/qr';
import { AttendanceRequest } from '../models/attendance';
import { ApiService } from '../../../services/api.service';

@Injectable()
export class AttendanceService {
  constructor(
    private _api: ApiService
  ) { }

  public async ScanAsync(
    request: AttendanceRequest
  ) {
    return await this._api.Post('Attendance', request);
  }

  public async UnassignAsync(
    qr: string
  ) {
    return await this._api.Delete(`Attendance/${qr}`);
  }

  public async GetQRsAsync() {
    return await this._api.Get<QRResponse[]>('Attendance/QR');
  }

  public async CheckAllAsync(
    day: boolean
  ) {
    return await this._api.Post(`Attendance/All?day=${day}`, undefined);
  }
}
