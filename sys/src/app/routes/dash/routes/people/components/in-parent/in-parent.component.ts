import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ParentRequest } from '../../models/requests/person';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'people-in-parent',
  standalone: false,

  templateUrl: './in-parent.component.html',
  styleUrl: './in-parent.component.sass'
})
export class InParentComponent {
  constructor(
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      gender: [true, Validators.required],
      phone: ['', [Validators.maxLength(9), Validators.minLength(9)]]
    });
  }

  @Input() parents: ParentRequest[] = [];
  @Output() parentsChange = new EventEmitter<ParentRequest[]>();
  form: FormGroup;

  get gender() { return `${this.form.controls['gender'].value}` === 'true' }
  private get _name() { return `${this.form.controls['name'].value}`.trim(); }
  private get _phone() { return `${this.form.controls['phone'].value}`.replace('-', ''); }

  Delete(
    parent: ParentRequest
  ) {
    const index = this.parents.findIndex(p => p == parent);
    this.parents.splice(index, 1);
    this.parentsChange.emit(this.parents);
  }

  Add() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    if (this.parents.length == 2) return;
    const parent: ParentRequest = {
      name: this._name,
      gender: this.gender,
      isParent: true,
      phone: this._phone !== '' ? this._phone : undefined
    };
    this.parents.push(parent);
    this.parentsChange.emit(this.parents);
    this.form.reset();
    this.form.controls['gender'].setValue(false);
    this.form.controls['phone'].setValue('');
  }

  FormatPhone() {
    const phone = `${this.form.controls['phone'].value}`.replace(/\D/g, '').slice(0, 8);
    if (!phone) {
      this.form.controls['phone'].setValue('', { EventEmitter: false });
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

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }
}