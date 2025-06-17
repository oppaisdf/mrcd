import { Component, Input } from '@angular/core';
import { DayResponse } from '../../responses/planner';

@Component({
  selector: 'planner-com-activities',
  standalone: false,
  templateUrl: './activities.html',
  styleUrl: './activities.sass'
})
export class Activities {
  @Input() day!: DayResponse;

  CloseModal() {
    this.day.day = -1;
  }
}
