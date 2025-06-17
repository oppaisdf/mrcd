import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CalendarService } from '../../services/calendar.service';
import { MonthResponse } from '../../responses/calendar';
import { DayResponse } from '../../responses/planner';

@Component({
  selector: 'planner-comp-calendar',
  standalone: false,
  templateUrl: './calendar.html',
  styleUrl: './calendar.sass'
})
export class Calendar implements OnInit {
  constructor(
    private _service: CalendarService
  ) { }

  isSearching = false;
  selectedMonth: MonthResponse = {
    name: '',
    value: 0
  };
  months: MonthResponse[] = [];
  selectedMonthIndex = 0;
  days: DayResponse[] = [];
  private _selectedDay: DayResponse = {
    day: -1
  };
  @Output() selectedDay = new EventEmitter<DayResponse>();

  async ngOnInit() {
    this.months = this._service.Months;
    const now = new Date();
    this.selectedMonthIndex = now.getMonth();
    this.selectedMonth = this.months[this.selectedMonthIndex];
    await this.FillMonth(this.selectedMonth);
  }

  async OnMonthChange(
    event: Event
  ) {
    const selectedMonth = this.months[parseInt((event.target as HTMLSelectElement).value, 10)];
    await this.FillMonth(selectedMonth);
  }

  async FillMonth(
    month: MonthResponse,
  ) {
    if (this.isSearching) return;
    this.isSearching = true;
    this._selectedDay = { day: -1 };
    const response = await this._service.GetAsync(month.value + 1);
    this.isSearching = false;
    if (!response.success) return;
    this.days.splice(0, this.days.length, ...response.data!);
  }

  SelectDay(
    day: DayResponse
  ) {
    this._selectedDay = day;
    this.selectedDay.emit({ ...day });
  }

  GetDayClass(
    day: DayResponse
  ) {
    const isSelected = this._selectedDay.day === day.day;
    const hasActivities = day.activities && day.activities.length > 0;
    return {
      'tag': isSelected,
      'has-skeleton has-text-weight-bold': isSelected,
      'has-text-white': isSelected && !hasActivities,
      'has-text-gray': !isSelected && !hasActivities,
      'has-text-warning': hasActivities
    };
  }
}
