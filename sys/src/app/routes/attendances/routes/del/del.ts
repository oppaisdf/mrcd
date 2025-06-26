import { Component, ViewChild } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';
import { ScannerComponent } from '../../components/scanner/scanner.component';

@Component({
  selector: 'attendances-del',
  standalone: false,
  templateUrl: './del.html',
  styleUrl: './del.sass'
})
export class Del {
  constructor(
    private _service: AttendanceService
  ) { }

  @ViewChild(ScannerComponent) private _scanner!: ScannerComponent
  message = '';
  success = false;
  loading = false;

  async ScanAsync(
    qr: string
  ) {
    if (this.loading) return;
    if (!qr) return;
    this.loading = true;
    const response = await this._service.UnassignAsync(qr);
    this.message = response.message;
    this.success = response.success;
    this.loading = false;
    this._scanner.Stop();
  }

  async StartScanAsync() {
    await this._scanner.StartAsync();
  }
}
