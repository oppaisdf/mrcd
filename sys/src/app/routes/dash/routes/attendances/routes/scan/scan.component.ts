import { Component, ViewChild } from '@angular/core';
import { BarcodeFormat } from '@zxing/browser';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';
import { AttendanceService } from '../../services/attendance.service';

@Component({
  selector: 'attendances-scan',
  standalone: false,
  templateUrl: './scan.component.html',
  styleUrl: './scan.component.sass'
})
export class ScanComponent {
  constructor(
    private _service: AttendanceService
  ) { }

  @ViewChild('scanner') scanner!: ZXingScannerComponent;
  message = '';
  success = true;
  isScanning = false;
  isAttendance = true;
  attendanceDate = new Date();
  formats: BarcodeFormat[] = [BarcodeFormat.QR_CODE];

  LimitDate(
    isMax: boolean
  ) {
    const now = new Date();
    if (isMax) now.setMonth(now.getMonth() + 1);
    else now.setMonth(now.getMonth() - 1);
    return now.toISOString().split('T')[0];
  }

  async Scan(
    qr: string
  ) {
    if (this.isScanning) return;
    this.isScanning = true;
    this.scanner.scanStop();

    const response = await this._service.ScanAsync(qr);
    this.message = response.message;
    this.success = response.success;
    this.isScanning = false;
  }

  StartScan() {
    this.scanner.scanStart();
  }
}
