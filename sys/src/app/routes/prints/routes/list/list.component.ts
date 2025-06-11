import { Component, OnDestroy, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { GeneralParentListResponse, ListGeneralResponse } from '../../responses/list';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'prints-list',
  standalone: false,

  templateUrl: './list.component.html',
  styleUrl: './list.component.sass'
})
export class ListComponent implements OnInit, OnDestroy {
  constructor(
    private _service: PrinterService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: [''],
      gender: [],
      day: [],
      orderBy: [true]
    });
  }

  private _people: ListGeneralResponse[] = [];
  private _subscriptions = new Subscription();
  form: FormGroup;
  people: ListGeneralResponse[] = [];
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
    this.people = this._people;
    this.OrderBy(true);
    this._subscriptions.add(
      this.form.valueChanges.subscribe(() => {
        this.Filter();
      })
    );
  }

  ngOnDestroy() {
    this._subscriptions.unsubscribe();
  }

  ClearFilters() {
    this.form.reset();
    this.form.patchValue({
      orderBy: 'true'
    });
    this.people = this._people;
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

    this.people = this._people.filter(q =>
      (name === '' || q.name.includes(name)) &&
      (day === undefined || q.day === day) &&
      (gender === undefined || q.gender === gender)
    );
    this.OrderBy(this.GetValue('orderBy') === 'true');
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

  private GetValue(
    control: string
  ) {
    return `${this.form.controls[control].value}`.replaceAll('<empty string>', '').replaceAll('null', '');
  }

  private OrderBy(
    isAlpha: boolean
  ) {
    if (isAlpha) this.people.sort((a, b) => a.name.localeCompare(b.name));
    else this.people.sort((a, b) => a.id - b.id);
  }
}
