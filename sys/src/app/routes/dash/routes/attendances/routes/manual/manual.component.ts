import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';
import { QRResponse } from '../../../prints/responses/qr';

@Component({
  selector: 'attendance-manual',
  standalone: false,
  templateUrl: './manual.component.html',
  styleUrl: './manual.component.sass'
})
export class ManualComponent implements OnInit {
  constructor(
    private _service: AttendanceService
  ) { }

  loading = false;
  message = '';
  success = true;
  qrs: QRResponse[] = [];
  _qrs: QRResponse[] = [];
  name = '';
  day = '1';
  gender = '1';

  async ngOnInit() {
    if (this.loading) return;
    this.loading = true;
    const response = await this._service.GetQRsAsync();
    this.message = response.message;
    this.success = response.success;
    this.loading = false;
    if (!this.success) return;
    this.qrs = response.data!;
    this._qrs = response.data!;
  }

  Filter() {
    const name = this.name;
    const day = this.day === '1' ? undefined : this.day === '2';
    const gender = this.gender === '1' ? undefined : this.gender === '2';

    if (name === '' && day === undefined && gender === undefined) {
      this.ClearFilters();
      return;
    }

    this.qrs = this._qrs.reduce((lst, q) => {
      switch (true) {
        case (day === undefined && gender === undefined):
          if (q.name.includes(name)) lst.push(q);
          break;
        case (day === undefined && name === ''):
          if (q.gender === gender) lst.push(q);
          break;
        case (gender === undefined && name === ''):
          if (q.day === day) lst.push(q);
          break;
        case (day === undefined && gender !== undefined && name !== ''):
          if (q.gender === gender && q.name.includes(name)) lst.push(q);
          break;
        case (gender === undefined && day !== undefined && name !== ''):
          if (q.day === day && q.name.includes(name)) lst.push(q);
          break;
        case (name === '' && gender !== undefined && day !== undefined):
          if (q.gender === gender && q.day === day) lst.push(q);
          break;
      }
      return lst;
    }, [] as QRResponse[]);
  }

  ClearFilters() {
    this.name = '';
    this.gender = '1';
    this.day = '1';
    this.qrs = this._qrs;
  }

  async AssistanceAsync(
    qr: string
  ) {
    if (this.loading) return;
    this.loading = true;
    const response = await this._service.ScanAsync(qr);
    this.message = response.message;
    this.success = response.success;
    this.loading = false;
  }
}
