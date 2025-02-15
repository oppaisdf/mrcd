import { Component, OnDestroy, OnInit } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';
import { QRResponse } from '../../../prints/responses/qr';
import { AttendanceRequest } from '../../models/attendance';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'attendance-manual',
  standalone: false,
  templateUrl: './manual.component.html',
  styleUrl: './manual.component.sass'
})
export class ManualComponent implements OnInit, OnDestroy {
  constructor(
    private _service: AttendanceService,
    private _form: FormBuilder
  ) {
    this.filters = this._form.group({
      name: [''],
      day: [],
      gender: []
    });
    this.form = this._form.group({
      isAttendance: [true],
      date: ['']
    });
  }

  loading = false;
  message = '';
  success = true;
  qrs: QRResponse[] = [];
  _qrs: QRResponse[] = [];
  filters: FormGroup;
  form: FormGroup;
  private _subscriptions = new Subscription();

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
    this._subscriptions.add(
      this.filters.valueChanges.subscribe(() => { this.Filter() })
    );
  }

  ngOnDestroy() {
    this._subscriptions.unsubscribe();
  }

  private Filter() {
    const name = this.GetValue('name');
    const day = (() => {
      switch (this.GetValue('day')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();
    const gender = (() => {
      switch (this.GetValue('gender')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();

    this.qrs = this._qrs.filter(q =>
    (
      (name === '' || q.name.includes(name)) &&
      (day === undefined || q.day === day) &&
      (gender === undefined || q.gender === gender)
    ));
  }

  ClearFilters() {
    this.filters.reset();
    this.qrs = this._qrs;
  }

  async AssistanceAsync(
    qr: string,
    isAttendance: boolean
  ) {
    if (this.loading) return;
    this.loading = true;

    const request: AttendanceRequest = {
      hash: qr,
      isAttendance: this.GetValue('isAttendance', true) === 'true',
      date: this.GetValue('date', true) !== '' ? this.GetValue('date', true) + 'T00:00:00' : undefined
    };
    const response = isAttendance ?
      await this._service.ScanAsync(request) :
      await this._service.UnassignAsync(qr);
    this.message = response.message;
    this.success = response.success;
    this.loading = false;
  }

  GetValue(
    control: string,
    isForm: boolean = false
  ) {
    if (!isForm) return `${this.filters.controls[control].value}`.replaceAll('<empty string>', '').replaceAll('null', '');
    else return `${this.form.controls[control].value}`.replaceAll('<empty string>', '').replaceAll('null', '');
  }
}
