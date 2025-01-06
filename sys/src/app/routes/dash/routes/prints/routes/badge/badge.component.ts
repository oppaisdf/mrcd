import { Component, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { QRResponse } from '../../responses/qr';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'prints-badge',
  standalone: false,
  templateUrl: './badge.component.html',
  styleUrl: './badge.component.sass'
})
export class BadgeComponent implements OnInit {
  constructor(
    private _service: PrinterService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: [''],
      day: [],
      gender: []
    });
  }

  qrs: QRResponse[] = [];
  form: FormGroup;
  private _qrs: QRResponse[] = [];

  private GetValue(
    control: string
  ) {
    return this.form.controls[control].value;
  }

  async ngOnInit() {
    const response = await this._service.GetQRsAsync();
    if (!response.success) return;
    this.qrs = response.data!;
    this._qrs = response.data!;
  }

  FilterBy(
    control: string
  ) {
    switch (control) {
      case 'day':
        const day = `${this.GetValue('day')}` === 'true';
        this.qrs = this._qrs.reduce((lst, q) => {
          if (q.day === day) lst.push(q);
          return lst;
        }, [] as QRResponse[]);
        break;
      case 'gender':
        const gender = `${this.GetValue('gender')}` === 'true';
        this.qrs = this._qrs.reduce((lst, q) => {
          if (q.gender === gender) lst.push(q);
          return lst;
        }, [] as QRResponse[]);
        break;
      default:
        const name = `${this.GetValue('name')}`;
        this.qrs = this._qrs.reduce((lst, q) => {
          if (q.name.includes(name)) lst.push(q);
          return lst;
        }, [] as QRResponse[]);
        break;
    }
  }

  ClearFilters() {
    this.form.reset();
    this.qrs = this._qrs;
  }
}
