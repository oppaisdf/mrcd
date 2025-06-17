import { Component } from '@angular/core';
import { DayResponse } from '../../models/responses';

@Component({
  selector: 'planner-core',
  standalone: false,
  templateUrl: './planner.html',
  styleUrl: './planner.sass'
})
export class Planner {
  day: DayResponse = { day: -1 };
  month = 0;
}
