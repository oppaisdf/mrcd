import { Component } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';

@Component({
  selector: 'attendances-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent {
  constructor(
    private _service: AttendanceService
  ) { }

  message = '';
  success = true;
  loading = false;
  typeAttendance = 0;

  SelectTypeAttendance(
    type: number
  ) {
    this.typeAttendance = type;
  }

  async CheckAllAsync() {
    if (this.loading) return;
    this.loading = true;
    const response = await this._service.CheckAllAsync();
    this.message = response.message;
    this.success = response.success;
    this.loading = false;
  }
}
