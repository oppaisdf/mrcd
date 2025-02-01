import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { QRResponse } from '../../prints/responses/qr';

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

  public async UnassignAsync(
    qr: string
  ) {
    return await this._api.Delete(`Attendance/${qr}`);
  }

  public async GetQRsAsync() {
    return await this._api.Get<QRResponse[]>('Attendance/QR');
  }

  public async CheckAllAsync() {
    return await this._api.Post('Attendance/All', undefined);
  }
}
