import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ParentRequest, PersonRequest } from '../../models/requests/person';
import { DefaultEntityResponse } from '../../models/responses/person';
import { environment } from '../../../../core/environments/environment';

@Component({
  selector: 'person-new',
  standalone: false,
  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent implements OnInit {
  constructor(
    private _service: PersonService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(65)]],
      gender: [true, Validators.required],
      dob: ['', Validators.required],
      day: [true, Validators.required],
      degreeId: [undefined, Validators.required],
      address: ['', [Validators.required, Validators.maxLength(100)]],
      phone: ['', [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      pay: [false]
    });
  }

  degrees: DefaultEntityResponse[] = [];
  currencySymbol = environment.currencySymbol;
  sacraments: DefaultEntityResponse[] = [];
  form: FormGroup;
  parentForm: FormGroup | undefined;
  parents: ParentRequest[] = [];
  private _sacraments: number[] = [];
  price = 0;
  message = '';
  success = true;
  isAdding = false;

  GetValue(
    control: string
  ) {
    return this.form.controls[control].value;
  }

  GetMDate(
    isMax: boolean
  ) {
    const now = new Date();
    if (isMax) return now.toISOString().split('T')[0];
    now.setFullYear(now.getFullYear() - 43);
    return now.toISOString().split('T')[0];
  }

  async ngOnInit() {
    const response = await this._service.GetFiltersAsync();
    if (!response.success) return;
    this.degrees = response.data!.degrees;
    this.sacraments = response.data!.sacraments;
    if (response.data!.price) this.price = response.data!.price;
  }

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  FormatPhone() {
    const phone = `${this.GetValue('phone')}`.replace(/\D/g, '').slice(0, 8);
    if (!phone) {
      this.form.controls['phone'].setValue('', { emitEvent: false });
      return;
    }
    if (phone.length < 5) {
      this.form.controls['phone'].setValue(phone, { emitEvent: false });
      return;
    }
    const first = phone.substring(0, 4);
    const last = phone.substring(4, 8);
    this.form.controls['phone'].setValue(`${first}-${last}`, { emitEvent: false });
  }

  SelectSacrament(
    id: number
  ) {
    const index = this._sacraments.findIndex(s => s === id);
    if (index < 0) this._sacraments.push(id);
    else this._sacraments.splice(index, 1);
  }

  async AddAsync() {
    if (this.isAdding) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.form.disable();
    this.isAdding = true;

    const request: PersonRequest = {
      name: this.GetValue('name'),
      gender: `${this.GetValue('gender')}` === 'true',
      dob: this.GetValue('dob'),
      day: `${this.GetValue('day')}` === 'true',
      degreeId: this.GetValue('degreeId'),
      address: this.GetValue('address'),
      phone: `${this.GetValue('phone')}`.replace(/\D/g, ''),
      parents: this.parents,
      sacraments: this._sacraments,
      pay: this.GetValue('pay')
    };
    const response = await this._service.AddAsync(request);
    this.message = response.message;
    this.success = response.success;

    this.form.enable();
    this.isAdding = false;
    if (!response.success) return;
    this.message = 'Se inscribió el confirmando correctamente';
    this.form.reset();
    this.form.patchValue({
      day: true,
      gender: true,
      pay: false
    });
    this.parents = [];
    if (!this.parentForm) return;
    this.parentForm.reset();
    this.parentForm.patchValue({
      name: '',
      gender: false,
      phone: ''
    });
  }
}
