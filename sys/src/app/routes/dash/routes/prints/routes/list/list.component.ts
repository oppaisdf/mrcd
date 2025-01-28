import { Component, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { GeneralParentListResponse, ListGeneralResponse } from '../../responses/list';

@Component({
  selector: 'prints-list',
  standalone: false,

  templateUrl: './list.component.html',
  styleUrl: './list.component.sass'
})
export class ListComponent implements OnInit {
  constructor(
    private _service: PrinterService
  ) { }

  private _people: ListGeneralResponse[] = [];
  people: ListGeneralResponse[] = [];
  name = '';
  gender = '1';
  day = '1';
  isVertical = true;
  showPhones = false;

  GetDOB(
    date: Date
  ) {
    const dob = new Date(date);
    return `${dob.getDate()} de ${dob.toLocaleString('es-ES', { month: 'long' })} del ${dob.getFullYear()}`;
  }

  async ngOnInit() {
    const response = await this._service.GetGeneralList();
    if (!response.success) return;
    this._people = response.data!;
    this._people.sort((a, b) => a.name.localeCompare(b.name));
    this.people = response.data!;
  }

  ClearFilters() {
    this.people = this._people;
    this.name = '';
    this.day = '1';
    this.gender = '1';
  }

  Filter() {
    const name = this.name;
    const day = this.day === '1' ? undefined : this.day === '2';
    const gender = this.gender === '1' ? undefined : this.gender === '2';

    if (name === '' && day === undefined && gender === undefined) {
      this.ClearFilters();
      return;
    }

    this.people = this._people.reduce((lst, q) => {
      switch (true) {
        case (day === undefined && gender === undefined):
          if (q.name.includes(name)) lst.push(q);
          break;
        case (day === undefined && name === ''):
          if (q.gender === gender) lst.push(q);
          break;
        case (gender === undefined && name === ''):
          if (q.day === day) lst.push(q);
          break;
        case (day === undefined && gender !== undefined && name !== ''):
          if (q.gender === gender && q.name.includes(name)) lst.push(q);
          break;
        case (gender === undefined && day !== undefined && name !== ''):
          if (q.day === day && q.name.includes(name)) lst.push(q);
          break;
        case (name === '' && gender !== undefined && day !== undefined):
          if (q.gender === gender && q.day === day) lst.push(q);
          break;
      }
      return lst;
    }, [] as ListGeneralResponse[]);
  }

  OnChangeShowPhones() {
    this.showPhones = `${this.showPhones}` === 'true';
  }

  ChangeOrientation() {
    this.isVertical = `${this.isVertical}` === 'true';
  }

  GetFormatedParents(
    parent: GeneralParentListResponse[] | undefined
  ) {
    if (!parent) return '';
    return parent.reduce((a, b) => {
      return a += `${a.length > 0 ? ', ' : ''}${b.name}${b.phone && this.showPhones ? `(${this.GetFormatedPhone(b.phone)})` : ''}`;
    }, '');
  }

  GetFormatedPhone(
    phone: string | undefined
  ) {
    if (!phone) return '';
    return `${phone.substring(0, 4)}-${phone.substring(4, 8)}`;
  }
}
