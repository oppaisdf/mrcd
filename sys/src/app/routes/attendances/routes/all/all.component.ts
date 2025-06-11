import { Component } from '@angular/core';

@Component({
  selector: 'attendances-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent {
  message = '';
  success = true;
  loading = false;
  typeAttendance = 0;

  SelectTypeAttendance(
    type: number
  ) {
    this.typeAttendance = type;
  }
}
