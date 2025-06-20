import { Component, OnInit } from '@angular/core';
import { DayResponse, SimpleActivityResponse } from '../../models/responses';
import { CalendarService } from '../../services/calendar.service';

@Component({
  selector: 'planner-core',
  standalone: false,
  templateUrl: './planner.html',
  styleUrl: './planner.sass'
})
export class Planner implements OnInit {
  constructor(
    private _service: CalendarService
  ) { }

  day: DayResponse = { day: -1 };
  month = 0;
  nextActivity: SimpleActivityResponse = {
    id: 0,
    name: ''
  };

  async ngOnInit() {
    const response = await this._service.NextActivityAsync();
    if (!response.success) return;
    if (response.data) this.nextActivity = response.data;
  }
}
