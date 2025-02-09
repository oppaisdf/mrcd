import { Component, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { QRResponse } from '../../responses/qr';

@Component({
  selector: 'prints-badge',
  standalone: false,
  templateUrl: './badge.component.html',
  styleUrl: './badge.component.sass'
})
export class BadgeComponent implements OnInit {
  constructor(
    private _service: PrinterService
  ) { }

  name = '';
  day = '1';
  gender = '1';
  isVertical = true;
  columns = '2';

  qrs: QRResponse[] = [];
  private _qrs: QRResponse[] = [];

  async ngOnInit() {
    const response = await this._service.GetQRsAsync();
    if (!response.success) return;
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

    this.qrs = this._qrs.filter(q =>
      (name === '' || q.name.includes(name)) &&
      (day === undefined || q.day === day) &&
      (gender === undefined || q.gender === gender)
    );
  }

  ClearFilters() {
    this.name = '';
    this.gender = '1';
    this.day = '1';
    this.qrs = this._qrs;
  }

  ChangeOrientation() {
    this.isVertical = `${this.isVertical}` === 'true';
  }
}
