import { Component } from '@angular/core';
import { DayResponse } from '../responses/planner';

@Component({
  selector: 'app-planner',
  standalone: false,
  templateUrl: './planner.html',
  styleUrl: './planner.sass'
})
export class Planner {
  day: DayResponse = { day: -1 };
}
