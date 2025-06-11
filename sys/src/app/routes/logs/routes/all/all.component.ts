import { Component, OnInit } from '@angular/core';
import { LogService } from '../../services/log.service';
import { ActionFilterResponse, LogResponse, UserFilterResponse } from '../../models/responses/logs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LogFilters } from '../../models/request/logs';

@Component({
  selector: 'logs-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: LogService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      page: [1],
      userId: [],
      action: [],
      start: [],
      end: []
    });
  }

  page = 1;
  lastPage = 1;
  logs: LogResponse[] = [];
  users: UserFilterResponse[] = [];
  actions: ActionFilterResponse[] = [];
  form: FormGroup;
  isSearching = false;
  messsage = '';
  success = true;
  maxDate = '';
  minEndDate = '';

  async ngOnInit() {
    this.maxDate = new Date().toISOString().split('T')[0];
    this.form.controls['start'].valueChanges.subscribe((start) => {
      this.minEndDate = start;
    });

    const response = await this._service.GetFiltersAsync();
    if (!response.success) return;
    this.users = response.data!.users;
    this.actions = response.data!.actions;
  }

  async FindAsync() {
    if (this.isSearching) return;
    this.isSearching = true;
    this.form.disable();

    const request: LogFilters = this.form.value;
    if (request.start) {
      const d = new Date(request.start);
      request.start = `${d.getFullYear()}-${d.getMonth() + 1 < 10 ? '0' : ''}${d.getMonth() + 1}-${d.getDate() < 10 ? '0' : ''}${d.getDay()}T00:00:00`;
    }
    if (request.end) {
      const d = new Date(request.end);
      request.start = `${d.getFullYear()}-${d.getMonth() + 1 < 10 ? '0' : ''}${d.getMonth() + 1}-${d.getDate() < 10 ? '0' : ''}${d.getDay()}T00:00:00`;
    }

    const response = await this._service.GetAsync(request);
    this.messsage = response.message;
    this.success = response.success;

    this.isSearching = false;
    this.form.enable();
    if (!this.success) return;
    this.messsage = '';
    const pages = response.message.split('/');
    this.page = +pages[0];
    this.lastPage = +pages[1];
    this.logs = response.data!;
  }

  GetDate(
    date: Date
  ) {
    const d = new Date(date);
    const day = d.toLocaleString('en-US', { day: 'numeric' });
    const month = d.toLocaleString('es-ES', { month: 'long' });

    return `${day} de ${month} del ${d.getFullYear()}`;
  }

  GetHour(
    date: Date
  ) {
    const d = new Date(date);
    return `${d.toLocaleString('en-US', { hour: '2-digit' })}`.replace(' ', `:${d.toLocaleString('en-US', { minute: '2-digit' })} `);;
  }

  async ChangePage(
    isNext: boolean
  ) {
    if (isNext) this.page += 1;
    else this.page += -1;
    this.form.controls['page'].setValue(this.page);
    await this.FindAsync();
  }

  ClearFilters() {
    this.form.patchValue({
      page: 1,
      userId: undefined,
      action: undefined,
      start: undefined,
      end: undefined
    });
    this.logs = [];
  }
}
