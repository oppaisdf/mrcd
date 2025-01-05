import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ParentResponse } from '../../models/responses/person';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PersonService } from '../../services/person.service';
import { PersonRequest } from '../../models/requests/person';

@Component({
  selector: 'people-comp-parents',
  standalone: false,
  templateUrl: './parents.component.html',
  styleUrl: './parents.component.sass'
})
export class ParentsComponent {
  constructor(
    private _form: FormBuilder,
    private _service: PersonService
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

  get gender() { return this.form.controls['gender'].value; }
  get phone() { return `${this.form.controls['phone'].value}`.replace(/\D/g, ''); }

  async AddAsync() {
    if (this.updating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.updating = true;
    this.updatingChange.emit(true);
    this.form.disable();

    this.parents.push(({
      name: this.form.controls['name'].value,
      gender: this.gender,
      phone: this.phone ? this.phone : undefined,
      id: 0
    }));

    const request: PersonRequest = {};
    if (this.isParent) request.parents = this.parents.map(p => ({ name: p.name, gender: p.gender, phone: p.phone }));
    else request.godparents = this.parents.map(p => ({ name: p.name, gender: p.gender }));
    const response = await this._service.UpdateAsync(this.id, request);
    this.message = response.message;
    this.success = response.success;

    this.updating = false;
    this.updatingChange.emit(false);
    this.form.enable();

    if (response.message) {
      this.form.reset();
      this.form.controls['gender'].setValue(false);
      return;
    }

    this.parents.pop();
    this.form.reset();
    this.form.controls['gender'].setValue(false);
  }

  async RemoveAsync(
    parent: ParentResponse
  ) {
    if (this.updating) return;
    this.updating = true;
    this.updatingChange.emit(true);
    this.form.disable();

    const index = this.parents.findIndex(p => p == parent);
    this.parents.splice(index, 1);
    const request: PersonRequest = {};
    if (this.isParent) request.parents = this.parents;
    else request.godparents = this.parents;

    const response = await this._service.UpdateAsync(this.id, request);
    this.message = response.message;
    this.success = response.success;

    this.form.enable();
    this.form.reset();
    this.form.controls['gender'].setValue(false);
    this.updating = false;
    this.updatingChange.emit(false);
    if (!response.success) this.parents.push(parent);
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
