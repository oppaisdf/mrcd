import { Component } from '@angular/core';
import { AttendanceService } from '../../services/attendance.service';

@Component({
  selector: 'attendances-all-day',
  standalone: false,
  templateUrl: './all-day.html',
  styleUrl: './all-day.sass'
})
export class AllDay {
  constructor(
    private _service: AttendanceService
  ) { }

  day = true;
  loading = false;
  message = '';
  success = false;

  ChangeDay() {
    this.day = `${this.day}` === 'true';
  }

  async CheckAllAsync() {
    if (this.loading) return;
    this.loading = true;

    const response = await this._service.CheckAllAsync(this.day);
    this.message = response.message;
    this.success = response.success;

    this.loading = false;
  }
}
