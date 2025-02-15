import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { QRResponse } from '../responses/qr';
import { ListGeneralResponse } from '../responses/list';
import { AttendanceResponse } from '../responses/attandence';

@Injectable()
export class PrinterService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetQRsAsync() {
    return await this._api.Get<QRResponse[]>('Attendance/QR');
  }

  public async GetGeneralList() {
    return await this._api.Get<ListGeneralResponse[]>('Attendance/List');
  }

  public async GetAsync() {
    return await this._api.Get<AttendanceResponse[]>('Attendance');
  }
}
