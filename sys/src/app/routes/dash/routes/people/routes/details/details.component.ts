import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PersonService } from '../../services/person.service';
import { DefaultEntityResponse, ParentResponse, SacramentResponse } from '../../models/responses/person';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PersonRequest } from '../../models/requests/person';

@Component({
  selector: 'person-details',
  standalone: false,
  templateUrl: './details.component.html',
  styleUrl: './details.component.sass'
})
export class DetailsComponent implements OnInit {
  constructor(
    private _me: ActivatedRoute,
    private _service: PersonService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      gender: [true, Validators.required],
      dob: ['', Validators.required],
      day: [true, Validators.required],
      isActive: [false],
      degreeId: [0, Validators.required],
      address: ['', [Validators.required, Validators.maxLength(100)]],
      phone: ['', [Validators.required, Validators.minLength(9), Validators.maxLength(9)]],
      parish: ['', Validators.maxLength(30)]
    });
  }

  id = 0;
  form: FormGroup;
  degrees: DefaultEntityResponse[] = [];
  sacraments: SacramentResponse[] = [];
  parents: ParentResponse[] = [];
  godparents: ParentResponse[] = [];
  private _old: PersonRequest = {};
  message = '';
  success = true;
  isUpdating = false;
  private _sacraments: number[] = [];

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

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
    now.setFullYear(now.getFullYear() - 30);
    return now.toISOString().split('T')[0];
  }

  async ngOnInit() {
    this.id = +this._me.snapshot.paramMap.get('id')!;
    const response = await this._service.GetByIdAsync(this.id);
    this.message = response.message;
    this.success = response.success;
    if (!response.success) return;

    this.degrees = response.data!.degrees!;
    if (response.data!.parents) this.parents = response.data!.parents;
    this.sacraments = response.data!.sacraments;
    if (response.data!.godparents) this.godparents = response.data!.godparents;
    this._sacraments = response.data!.sacraments.reduce((lst, s) => {
      if (s.isActive) lst.push(s);
      return lst;
    }, [] as SacramentResponse[]).map(s => s.id);

    this.form.patchValue({
      name: response.data!.name,
      gender: response.data!.gender,
      dob: response.data!.dob.toString().split('T')[0],
      day: response.data!.day,
      isActive: response.data!.isActive,
      degreeId: response.data!.degreeId,
      address: response.data!.address,
      phone: response.data!.phone,
      parish: response.data!.parish
    });
    this._old.name = this.GetValue('name');
    this._old.gender = this.GetValue('gender');
    this._old.dob = this.GetValue('dob');
    this._old.day = this.GetValue('day');
    this._old.isActive = this.GetValue('isActive');
    this._old.degreeId = this.GetValue('degreeId');
    this._old.address = this.GetValue('address');
    this._old.phone = this.GetValue('phone');
    this._old.parish = this.GetValue('parish');

    this.FormatPhone();
    if (response.data!.isActive) return;
    this.form.disable();
    this.form.controls['isActive'].enable();
  }

  SelectSacrament(
    sacrament: SacramentResponse
  ) {
    const index = this._sacraments.findIndex(s => s === sacrament.id);
    sacrament.isActive = index > -1;
    if (index < 0) this._sacraments.push(sacrament.id);
    else this._sacraments.splice(index, 1);
  }

  FormatPhone() {
    const phone = `${this.form.controls['phone'].value}`.replace(/\D/g, '').substring(0, 8);
    if (!phone) {
      this.form.controls['phone'].setValue('', { emitEvent: false });
      return;
    }

    const v1 = phone.substring(0, 4);
    const v2 = phone.substring(4, 8);
    if (phone.length < 5) this.form.controls['phone'].setValue(v1, { emitEvent: false });
    else this.form.controls['phone'].setValue(`${v1}-${v2}`, { emitEvent: false });
  }

  async UpdateAsync() {
    if (this.isUpdating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isUpdating = true;
    this.form.disable();

    const request: PersonRequest = {
      name: this.GetValue('name') !== this._old.name ? this.GetValue('name') : undefined,
      gender: this.GetValue('gender') !== this._old.gender ? this.GetValue('gender') : undefined,
      dob: this.GetValue('dob') !== this._old.dob ? this.GetValue('dob') : undefined,
      day: this.GetValue('day') !== this._old.day ? this.GetValue('day') : undefined,
      isActive: this.GetValue('isActive') !== this._old.isActive ? this.GetValue('isActive') : undefined,
      degreeId: this.GetValue('degreeId') !== this._old.degreeId ? this.GetValue('degreeId') : undefined,
      address: this.GetValue('address') !== this._old.address ? this.GetValue('address') : undefined,
      phone: this.GetValue('phone').replace(/\D/g, '') !== this._old.phone ? this.GetValue('phone').replace(/\D/g, '') : undefined,
      parish: this.GetValue('parish') !== this._old.parish ? this.GetValue('parish') : undefined,
      sacraments: this._sacraments
    };
    if (!request.phone || request.phone.length < 8) request.phone = undefined;
    const response = await this._service.UpdateAsync(this.id, request);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
    if (!response.success) {
      if (!this._old.isActive) this.form.controls['isActive'].enable();
      return;
    }
    if (!this.GetValue('isActive'))
      this.form.controls['isActive'].enable()
    else this.form.enable();

    if (this.GetValue('name') !== this._old.name) this._old.name = this.GetValue('name');
    if (this.GetValue('gender') !== this._old.gender) this._old.gender = this.GetValue('gender');
    if (this.GetValue('dob') !== this._old.dob) this._old.dob = this.GetValue('dob');
    if (this.GetValue('day') !== this._old.day) this._old.day = this.GetValue('day');
    if (this.GetValue('isActive') !== this._old.isActive) this._old.isActive = this.GetValue('isActive');
    if (this.GetValue('degreeId') !== this._old.degreeId) this._old.degreeId = this.GetValue('degreeId');
    if (this.GetValue('address') !== this._old.address) this._old.address = this.GetValue('address');
    if (this.GetValue('phone') !== this._old.phone) this._old.phone = this.GetValue('phone');
    if (this.GetValue('parish') !== this._old.parish) this._old.parish = this.GetValue('parish');
  }
}
