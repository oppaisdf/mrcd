import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { DefaultEntityResponse, PersonResponse } from '../../models/responses/person';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'person-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: PersonService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      page: [1],
      gender: [],
      day: [],
      isActive: [true],
      degreeId: [],
      name: ['', Validators.maxLength(30)]
    });
  }

  form: FormGroup;
  degrees: DefaultEntityResponse[] = [];
  people: PersonResponse[] = [];
  isSearching = false;
  message = 'Buscar por nombre puede sobrecargar el servidor, úselo lo menos posible';
  success = true;
  page = 1;
  lastPage = 1;

  GetValue(
    control: string
  ) {
    return this.form.controls[control].value;
  }

  async ngOnInit() {
    const response = await this._service.GetFiltersAsync();
    if (response.success) this.degrees = response.data!.degrees;
  }

  ClearFilters() {
    if (this.isSearching) return;
    this.form.reset();
    this.people = [];
    this.form.controls['isActive'].setValue(true);
  }

  async FillAsync() {
    if (this.isSearching) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isSearching = true;
    this.form.disable();

    const response = await this._service.GetAsync(this.form.value);
    this.message = response.message;
    this.success = response.success;

    this.isSearching = false;
    this.form.enable();
    if (!response.success) return;

    this.people = response.data!;
    this.message = '';
    const pages = response.message.split('/');
    this.page = +pages[0];
    this.lastPage = +pages[1]
  }

  async ChangePage(
    isNext: boolean
  ) {
    if (isNext) this.page = + 1;
    else this.page -= 1;
    this.form.controls['page'].setValue(this.page);
    await this.FillAsync();
  }
}
