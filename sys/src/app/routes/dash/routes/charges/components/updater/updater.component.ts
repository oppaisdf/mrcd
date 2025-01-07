import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ChargeService } from '../../services/charge.service';
import { ChargeResponse } from '../../models/responses/charge';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'charge-updater',
  standalone: false,
  templateUrl: './updater.component.html',
  styleUrl: './updater.component.sass'
})
export class UpdaterComponent {
  constructor(
    private _form: FormBuilder,
    private _service: ChargeService
  ) {
    this.form = this._form.group({
      name: ['', Validators.maxLength(11)],
      total: [0]
    });
  }

  @Input() show = false;
  @Output() showChange = new EventEmitter<boolean>();
  @Input() record: ChargeResponse = {
    id: 0,
    name: '',
    total: 0,
    isActive: true
  };

  form: FormGroup;
  isUpdating = false;
  message = '';
  success = true;
  currencySymbol = environment.currencySymbol;

  IsInvalidField(
    name: string
  ) {
    const control = this.form.controls[name];
    return control.touched && control.invalid;
  }

  Hide() {
    this.show = false;
    this.showChange.emit(this.show);
  }

  async UpdateAsync() {
    if (this.isUpdating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isUpdating = true;
    this.form.disable();

    const response = await this._service.UpdateAsync(this.record.id, this.form.value);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
    this.form.enable();
    if (response.success) window.location.reload();
  }
}
