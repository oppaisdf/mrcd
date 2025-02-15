import { Component, OnDestroy, OnInit } from '@angular/core';
import { PrinterService } from '../../services/printer.service';
import { QRResponse } from '../../responses/qr';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'prints-badge',
  standalone: false,
  templateUrl: './badge.component.html',
  styleUrl: './badge.component.sass'
})
export class BadgeComponent implements OnInit, OnDestroy {
  constructor(
    private _service: PrinterService,
    private _form: FormBuilder
  ) {
    this.filters = this._form.group({
      name: [''],
      day: [],
      gender: []
    });
    this.page = this._form.group({
      columns: ['2'],
      isVertical: ['true'],
      size: ['3'],
      sqr: ['2']
    });
  }

  private _subscriptions = new Subscription();
  filters: FormGroup;
  page: FormGroup;

  qrs: QRResponse[] = [];
  private _qrs: QRResponse[] = [];

  async ngOnInit() {
    const response = await this._service.GetQRsAsync();
    if (!response.success) return;
    this.qrs = response.data!;
    this._qrs = response.data!;

    this._subscriptions.add(
      this.filters.valueChanges.subscribe(() => {
        this.Filter();
      })
    );
  }

  ngOnDestroy() {
    this._subscriptions.unsubscribe();
  }

  Filter() {
    const name = this.GetValue(true, 'name');
    const day = (() => {
      switch (this.GetValue(true, 'day')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();

    const gender = (() => {
      switch (this.GetValue(true, 'gender')) {
        case 'true': return true;
        case 'false': return false;
        default: return undefined;
      }
    })();

    this.qrs = this._qrs.filter(q =>
      (name === '' || q.name.includes(name)) &&
      (day === undefined || q.day === day) &&
      (gender === undefined || q.gender === gender)
    );
  }

  ClearFilters() {
    this.filters.reset();
    this.qrs = this._qrs;
  }

  GetValue(
    isFilter: boolean,
    control: string
  ) {
    if (isFilter) return `${this.filters.controls[control].value}`.replaceAll('<empty string>', '').replaceAll('null', '');
    else return `${this.page.controls[control].value}`;
  }
}
