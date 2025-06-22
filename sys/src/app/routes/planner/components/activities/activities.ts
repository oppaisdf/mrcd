import { Component, Input } from '@angular/core';
import { DayResponse } from '../../models/responses';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CalendarService } from '../../services/calendar.service';
import { ActivityRequest } from '../../models/requests';
import { Router } from '@angular/router';

@Component({
  selector: 'planner-com-activities',
  standalone: false,
  templateUrl: './activities.html',
  styleUrl: './activities.sass'
})
export class Activities {
  constructor(
    private _form: FormBuilder,
    private _service: CalendarService,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(50)]]
    });
  }

  @Input() day!: DayResponse;
  @Input() month!: number;
  form: FormGroup;
  isCreating = false;
  success = false;
  message = '';

  CloseModal() {
    this.day.day = -1;
  }

  InvalidaField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  GetValue(
    control: string
  ) {
    return `${this.form.controls[control].value}`.trim();
  }

  async CreateActivityAsync() {
    if (this.isCreating) return;
    const name = this.GetValue('name');
    if (name === '') this.form.controls['name'].setValue('');
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isCreating = true;
    this.form.disable();

    const now = new Date();
    const date = new Date(Date.UTC(now.getUTCFullYear(), this.month, this.day.day));
    const request: ActivityRequest = {
      name: name,
      date: date
    };
    const response = await this._service.CreateActivityAsync(request);
    this.message = response.message;
    this.success = response.success;

    this.isCreating = false;
    this.form.enable();
    if (!this.success) return;
    this._router.navigateByUrl(`/planner/${response.data!.id}`);
  }
}
