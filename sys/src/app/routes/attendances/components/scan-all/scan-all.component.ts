import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';

@Component({
  selector: 'attendances-comp-scan-all',
  standalone: false,
  templateUrl: './scan-all.component.html',
  styleUrl: './scan-all.component.sass'
})
export class ScanAllComponent {
  constructor(
    private _service: AttendanceService
  ) { }

  @Input() message = '';
  @Input() success = true;
  @Input() loading = false;
  @Input() typeAttendance = 1;
  @Output() messageChange = new EventEmitter<string>();
  @Output() successChange = new EventEmitter<boolean>();
  @Output() loadingChange = new EventEmitter<boolean>();
  @Output() typeAttendanceChange = new EventEmitter<number>();
  day = true;

  async CheckAllAsync() {
    if (this.loading) return;
    this.loading = true;
    this.loadingChange.emit(true);

    const response = await this._service.CheckAllAsync(this.day);
    this.message = response.message;
    this.success = response.success;
    this.messageChange.emit(this.message);
    this.successChange.emit(this.success);

    this.loading = false;
    this.loadingChange.emit(false);
    this.Hide();
  }

  Hide() {
    this.typeAttendance = 0;
    this.typeAttendanceChange.emit(this.typeAttendance);
  }

  ChangeDay() {
    this.day = `${this.day}` === 'true';
  }
}
