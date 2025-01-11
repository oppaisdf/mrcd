import { Component, OnInit } from '@angular/core';
import { ParentService } from '../../services/parent.service';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BasicPersonResponse } from '../../../charges/models/responses/charge';
import { ParentRequest } from '../../../people/models/requests/person';

@Component({
  selector: 'parent-detail',
  standalone: false,
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.sass'
})
export class DetailComponent implements OnInit {
  constructor(
    private _service: ParentService,
    private _me: ActivatedRoute,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(30)]],
      gender: [true, Validators.required],
      phone: ['', [Validators.maxLength(9), Validators.minLength(9)]]
    });
  }

  form: FormGroup;
  id = 0;
  message = '';
  success = true;
  isUpdating = false;
  people: BasicPersonResponse[] = [];
  private _old: ParentRequest = {};

  async ngOnInit() {
    this.id = +this._me.snapshot.paramMap.get('id')!;
    const response = await this._service.GetByIdAsync(this.id);
    this.message = response.message;
    this.success = response.success;
    if (!response.success) return;
    this.form.patchValue({
      name: response.data!.name,
      gender: response.data!.gender,
      phone: response.data!.phone
    });
    this.FormatPhone();
    this._old = this.form.value;
    if (response.data!.people) this.people = response.data!.people;
  }

  GetValue(
    control: string
  ) {
    return `${this.form.controls[control].value}`;
  }

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  FormatPhone() {
    const phone = this.GetValue('phone').replace(/\D/g, '').substring(0, 8);
    if (!phone) {
      this.form.controls['phone'].setValue('', { emitEvent: false });
      return;
    }
    if (phone.length < 5)
      this.form.controls['phone'].setValue(phone, { emitEvent: false });
    else
      this.form.controls['phone'].setValue(`${phone.substring(0, 4)}-${phone.substring(4, 8)}`, { emitEvent: false });
  }

  async DeleteAsync() {
    if (this.isUpdating) return;
    this.isUpdating = true;

    const response = await this._service.DeleteAsync(this.id);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
  }

  async UpdateAsync() {
    if (this.isUpdating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isUpdating = true;
    this.form.disable();

    const request: ParentRequest = {
      name: this._old.name !== this.GetValue('name').trim() ? this.GetValue('name').trim() : undefined,
      phone: this._old.phone !== this.GetValue('phone').replace('-', '') ? this.GetValue('phone').replace('-', '') : undefined,
      gender: this._old.gender !== (this.GetValue('gender') === 'true') ? (this.GetValue('gender') === 'true') : undefined
    };
    const response = await this._service.UpdateAsync(this.id, request);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
    this.form.enable();
    if (!response.success) return;
    if (this._old.name !== this.GetValue('name').trim()) this._old.name = this.GetValue('name').trim();
    if (this._old.gender !== (this.GetValue('gender') === 'true')) this._old.gender = (this.GetValue('gender') === 'true');
    if (this._old.phone !== this.GetValue('phone').replace('-', '')) this._old.phone = this.GetValue('phone').replace('-', '');
  }
}
