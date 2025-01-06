import { Component, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { ListGeneralResponse } from '../../responses/list';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'prints-list',
  standalone: false,

  templateUrl: './list.component.html',
  styleUrl: './list.component.sass'
})
export class ListComponent implements OnInit {
  constructor(
    private _service: PrinterService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: [''],
      gender: [],
      day: []
    });
  }

  private _list: ListGeneralResponse[] = [];
  people: ListGeneralResponse[] = [];
  form: FormGroup;

  GetDOB(
    date: Date
  ) {
    const dob = new Date(date);
    return `${dob.getDate()} de ${dob.toLocaleString('es-ES', { month: 'long' })} del ${dob.getFullYear()}`;
  }

  GetValue(
    control: string
  ) {
    return this.form.controls[control].value;
  }

  async ngOnInit() {
    const response = await this._service.GetGeneralList();
    if (!response.success) return;
    this._list = response.data!;
    this._list.sort((a, b) => a.name.localeCompare(b.name));
    this.people = response.data!;
  }

  ClearFilters() {
    this.form.reset();
    this.people = this._list;
  }

  FilterBy(
    control: string
  ) {
    switch (control) {
      case 'gender':
        const gender = `${this.GetValue('gender')}` === 'true';
        this.people = this._list.reduce((lst, p) => {
          if (p.gender === gender) lst.push(p);
          return lst;
        }, [] as ListGeneralResponse[]);
        break;
      case 'day':
        const day = `${this.GetValue('day')}` === 'true';
        this.people = this._list.reduce((lst, p) => {
          if (p.day === day) lst.push(p);
          return lst;
        }, [] as ListGeneralResponse[]);
        break;
      default:
        const name = `${this.GetValue('name')}`;
        if (!name) break;
        this.people = this._list.reduce((lst, p) => {
          if (p.name.includes(name)) lst.push(p);
          return lst;
        }, [] as ListGeneralResponse[]);
        break;
    }
  }
}
