import { Component } from '@angular/core';
import { ParentService } from '../../services/parent.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ParentRequest } from '../../../people/models/requests/person';

@Component({
  selector: 'parents-new',
  standalone: false,
  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent {
  constructor(
    private _service: ParentService,
    private _form: FormBuilder,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(30)]],
      gender: [true, Validators.required],
      phone: ['', [Validators.maxLength(9), Validators.minLength(9)]]
    });
  }

  form: FormGroup;
  message = '';
  success = true;
  isCreating = false;

  GetValue(
    control: string
  ) {
    return this.form.controls[control].value;
  }

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  FormatPhone() {
    const phone = `${this.GetValue('phone')}`.replace(/\D/g, '').substring(0, 8);
    if (!phone) {
      this.form.controls['phone'].setValue('', { emitEvent: false });
      return;
    }
    if (phone.length < 5)
      this.form.controls['phone'].setValue(phone, { emitEvent: false });
    else
      this.form.controls['phone'].setValue(`${phone.substring(0, 4)}-${phone.substring(4, 8)}`, { emitEvent: false });
  }

  async AddAsync() {
    if (this.isCreating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isCreating = false;
    this.form.disable();

    const request: ParentRequest = {
      name: `${this.GetValue('name')}`.trim(),
      gender: `${this.GetValue('gender')}` === 'true',
      isParent: true,
      phone: `${this.GetValue('phone')}`.replace('-', '')
    };
    const response = await this._service.CreateAsync(request);
    this.message = response.message;
    this.success = response.success;

    this.isCreating = false;
    this.form.enable();
    if (response.success) this._router.navigateByUrl(`/parent/${response.data!.id}`);
  }
}
