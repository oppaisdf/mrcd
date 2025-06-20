import { Component, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { ActivityResponse } from '../../../planner/models/responses';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'prints-planner',
  standalone: false,
  templateUrl: './planner.html',
  styleUrl: './planner.sass'
})
export class Planner implements OnInit {
  constructor(
    private _service: PrinterService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      isVertical: ['true'],
      columns: ['1'],
      inLine: ['false']
    });
  }

  activities: ActivityResponse[] = [];
  form: FormGroup;

  async ngOnInit() {
    const response = await this._service.GetAllActivitiesAsync();
    if (!response.success) return;
    this.activities = response.data!;
  }

  GetDate(
    date: Date
  ) {
    const d = new Date(date);
    return `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`;
  }

  GetValue(
    name: string
  ) {
    return `${this.form.controls[name].value}`.trim();
  }

  get ColumnsClass() {
    const columns = +this.GetValue('columns');
    return {
      'is-full': columns == 1,
      'is-half': columns == 2,
      'is-one-third': columns == 3
    };
  }

  ColumnFieldClass(
    isTitle: boolean
  ) {
    const inLine = this.GetValue('inLine') === 'true';
    return {
      'is-full': !inLine,
      'is-one-third': isTitle && inLine,
      'is-two-thirds': !isTitle && inLine
    };
  }
}
