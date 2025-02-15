import { Component, EventEmitter, Input, Output } from '@angular/core';
import { BarcodeFormat } from '@zxing/browser';
import { AttendanceService } from '../../services/attendance.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AttendanceRequest } from '../../models/attendance';

@Component({
  selector: 'attendances-comp-scan',
  standalone: false,
  templateUrl: './scan.component.html',
  styleUrl: './scan.component.sass'
})
export class ScanComponent {
  constructor(
    private _service: AttendanceService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      isAttendance: [true],
      date: ['']
    });
  }

  @Input() isAttendance = true;
  @Input() loading = false;
  @Input() message = '';
  @Input() success = true;
  @Input() typeAttendance = 2;
  @Output() loadingChange = new EventEmitter<boolean>();
  @Output() messageChange = new EventEmitter<string>();
  @Output() successChange = new EventEmitter<boolean>();
  @Output() typeAttendanceChange = new EventEmitter<number>();
  formats: BarcodeFormat[] = [BarcodeFormat.QR_CODE];
  form: FormGroup;

  async Scan(
    qr: string
  ) {
    if (this.loading) return;
    this.loading = true;
    this.loadingChange.emit(true);

    const request: AttendanceRequest = {
      hash: qr,
      isAttendance: this.GetValue('isAttendance') === 'true' || this.GetValue('isAttendance') === 'false' ? this.GetValue('isAttendance') === 'true' : undefined,
      date: this.GetValue('date') !== '' ? this.GetValue('date') : undefined
    };
    const response = this.isAttendance ?
      await this._service.ScanAsync(request) :
      await this._service.UnassignAsync(qr);
    this.message = response.message;
    this.success = response.success;
    this.messageChange.emit(this.message);
    this.successChange.emit(this.success);
    this.loading = false;
    this.loadingChange.emit(false);
    this.CloseModal();
  }

  CloseModal() {
    this.typeAttendance = 0;
    this.typeAttendanceChange.emit(0);
  }

  GetValue(
    control: string
  ) {
    return `${this.form.controls[control].value}`.replaceAll('<empty string>', '').replaceAll('null', '');
  }
}
