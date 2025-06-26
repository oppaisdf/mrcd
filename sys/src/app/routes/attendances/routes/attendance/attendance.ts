import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AttendanceRequest } from '../../models/attendance';
import { AttendanceService } from '../../services/attendance.service';
import { ScannerComponent } from '../../components/scanner/scanner.component';

@Component({
  selector: 'attendances-attendance',
  standalone: false,
  templateUrl: './attendance.html',
  styleUrl: './attendance.sass'
})
export class Attendance implements OnInit {
  constructor(
    private _me: ActivatedRoute,
    private _form: FormBuilder,
    private _service: AttendanceService
  ) {
    this.form = this._form.group({
      isAttendance: ['true'],
      date: [''],
      alwaysShow: ['false']
    });
  }

  isAttendance = true;
  message = '';
  success = false;
  form: FormGroup;
  loading = false;
  @ViewChild(ScannerComponent) private _scanner!: ScannerComponent;

  ngOnInit() {
    this.isAttendance = this._me.snapshot.paramMap.get('isAttendance')?.toLowerCase() === 'true';
  }

  GetValue(
    control: string
  ) {
    return `${this.form.controls[control].value}`.replaceAll('<empty string>', '').replaceAll('null', '');
  }

  async ScanAsync(
    qr: string
  ) {
    if (this.loading) return;
    if (!qr) return;
    this._scanner.Pause();
    this.loading = true;

    const request: AttendanceRequest = {
      hash: qr,
      isAttendance: this.GetValue('isAttendance') === 'true',
      date: this.GetValue('date') !== '' ? this.GetValue('date') : undefined
    };
    const response = await this._service.ScanAsync(request);
    this.message = response.message;
    this.success = response.success;
    this.loading = false;
    if (this.GetValue('alwaysShow') === 'false') {
      this._scanner.Stop();
      return;
    }

    await this._scanner.StartAsync();
  }

  async StartScanAsync() {
    await this._scanner.StartAsync();
  }
}
