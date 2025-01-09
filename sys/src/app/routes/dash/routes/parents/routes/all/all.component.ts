import { Component } from '@angular/core';
import { ParentService } from '../../services/parent.service';
import { ParentResponse } from '../../models/response';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ParentFilter } from '../../models/requests';

@Component({
  selector: 'parents-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent {
  constructor(
    private _service: ParentService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: ['', Validators.maxLength(30)],
      gender: [],
      parent: []
    });
  }

  parents: ParentResponse[] = [];
  form: FormGroup;
  page = 0;
  lastPage = 0;
  isSearching = false;
  message = 'Buscar por nombre puede sobrecargar el servidor, úselo lo menos posible';
  success = true;

  private GetValue(
    control: string
  ) {
    return `${this.form.controls[control].value}`;
  }

  async ChangePage(
    isNext: boolean
  ) {
    if (isNext) this.page = + 1;
    else this.page -= 1;
    this.form.controls['page'].setValue(this.page);
    await this.Search();
  }

  ClearFilters() {
    this.form.reset();
    this.form.controls['name'].setValue('');
  }

  async Search() {
    if (this.isSearching) return;
    this.form.disable();
    this.isSearching = true;

    const filters: ParentFilter = {
      page: this.page,
      name: this.GetValue('name').trim() === '' ? undefined : this.GetValue('name').trim(),
      gender: this.GetValue('gender') !== 'true' && this.GetValue('gender') !== 'false' ? undefined : this.GetValue('gender') === 'true',
      isParent: this.GetValue('parent') !== 'true' && this.GetValue('parent') !== 'false' ? undefined : this.GetValue('parent') === 'true'
    };
    const response = await this._service.GetAsync(filters);
    this.message = response.message;
    this.success = response.success;

    this.form.enable();
    this.isSearching = false;
    if (!response.success) return;
    this.parents = response.data!;
    const pages = response.message.split('/');
    this.page = +pages[0];
    this.lastPage = +pages[1];
    this.message = '';
  }
}
