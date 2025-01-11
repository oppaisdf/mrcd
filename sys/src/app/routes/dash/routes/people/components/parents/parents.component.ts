import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ParentRequest } from '../../models/requests/person';
import { ParentService } from '../../services/parent.service';
import { ParentResponse } from '../../../parents/models/response';

@Component({
  selector: 'people-comp-parents',
  standalone: false,
  templateUrl: './parents.component.html',
  styleUrl: './parents.component.sass'
})
export class ParentsComponent {
  constructor(
    private _form: FormBuilder,
    private _service: ParentService
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(30)]],
      gender: [true, Validators.required],
      phone: ['', [Validators.minLength(9), Validators.maxLength(9)]]
    });
  }

  @Input() id = 0;
  @Input() isParent = false;
  @Input() updating = false;
  @Output() updatingChange = new EventEmitter<boolean>();
  @Input() parents: ParentResponse[] = [];
  form: FormGroup;
  message = '';
  success = true;

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  get gender() { return `${this.form.controls['gender'].value}` === 'true'; }
  get name() { return `${this.form.controls['name'].value}`.trim(); }
  get phone() { return `${this.form.controls['phone'].value}`.replace(/\D/g, ''); }

  async AddAsync() {
    if (this.updating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.updating = true;
    this.updatingChange.emit(true);
    this.form.disable();

    const request: ParentRequest = {
      name: this.name,
      gender: this.gender,
      phone: this.phone.length > 0 ? this.phone : undefined
    };
    const response = await this._service.CreateAsync(this.id, request);
    this.message = response.message;
    this.success = response.success;

    this.updating = false;
    this.updatingChange.emit(this.updating);
    this.form.enable();
    if (!response.success) return;
    const parent: ParentResponse = {
      id: response.data!.id,
      name: this.name,
      gender: this.gender,
      phone: this.phone,
      isParent: this.isParent
    };
    this.parents.push(parent);
  }

  async RemoveAsync(
    parent: ParentResponse
  ) {
    if (this.updating) return;
    this.updating = true;
    this.updatingChange.emit(true);
    this.form.disable();

    const response = await this._service.UnassignAsync(this.id, parent.id);
    this.message = response.message;
    this.success = response.success;

    this.updating = false;
    this.updatingChange.emit(this.updating);
    this.form.enable();
    if (!response.success) return;
    const index = this.parents.findIndex(p => p === parent);
    this.parents.splice(index, 1);
  }

  FormatNumber() {
    const phone = `${this.form.controls['phone'].value}`.replace(/\D/g, '').substring(0, 8);
    if (!phone) {
      this.form.controls['phone'].setValue('', { emitEvent: false })
      return;
    };

    if (phone.length < 5) this.form.controls['phone'].setValue(phone, { emitEvent: false });
    else this.form.controls['phone'].setValue(`${phone.substring(0, 4)}-${phone.substring(4, 8)}`);
  }
}
