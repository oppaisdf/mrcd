import { Component, OnDestroy, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { AttendanceResponse } from '../../responses/attandence';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'prints-attendance',
  standalone: false,
  templateUrl: './attendance.component.html',
  styleUrl: './attendance.component.sass'
})
export class AttendanceComponent implements OnInit, OnDestroy {
  constructor(
    private _service: PrinterService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      isVertical: [true]
    });
    this.filters = this._form.group({
      day: [],
      gender: [],
      name: ['']
    });
  }

  attendances: AttendanceResponse[] = [];
  private _attendances: AttendanceResponse[] = [];
  form: FormGroup;
  filters: FormGroup;
  private _subscriptions = new Subscription();
  headers: string[] = [];

  async ngOnInit() {
    const response = await this._service.GetAsync();
    if (!response.success) return;
    this._attendances = response.data!;
    this._attendances.sort((a, b) => a.name.localeCompare(b.name));
    this.attendances = this._attendances;
    this._subscriptions.add(
      this.filters.valueChanges.subscribe(() => this.Filter())
    );
    if (this._attendances.length > 0)
      this.headers = this._attendances[0].dates.map(d => `${d.day}-${d.month}`);
  }

  ngOnDestroy() {
    this._subscriptions.unsubscribe();
  }

  GetValue(
    control: string,
    fromPage: boolean = false
  ) {
    if (!fromPage) return `${this.filters.controls[control].value}`.replaceAll('<empty string>', '').replaceAll('null', '');
    else return `${this.form.controls[control].value}`;
  }

  Filter() {
    const name = this.GetValue('name');
    const day = (() => {
      switch (this.GetValue('day')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();

    const gender = (() => {
      switch (this.GetValue('gender')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();

    this.attendances = this._attendances.filter(a =>
      (name === '' || a.name.includes(name)) &&
      (day === undefined || a.day === day) &&
      (gender === undefined || a.gender === gender)
    );
  }

  ClearFilters() {
    this.filters.reset();
    this.attendances = this._attendances;
  }
}
