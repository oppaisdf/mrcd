import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { QRResponse } from '../responses/qr';

@Injectable()
export class PrinterService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetQRsAsync() {
    return await this._api.Get<QRResponse[]>('Attendance/QR');
  }
}
