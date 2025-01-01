import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DefaultResponse } from '../../../routes/dash/models/Response';
import { UpdateService } from '../../services/update.service';

@Component({
  selector: 'shared-updater',
  standalone: false,
  templateUrl: './updater.component.html',
  styleUrl: './updater.component.sass'
})
export class UpdaterComponent {
  constructor(
    private _form: FormBuilder,
    private _service: UpdateService
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(10)]]
    });
  }

  @Input() show = false;
  @Output() showChange = new EventEmitter<boolean>();
  @Input() endpoint = '';
  @Input() record: DefaultResponse = {
    id: 0,
    name: ''
  };

  form: FormGroup;
  isUpdating = false;
  message = '';
  success = true;

  get isInvalidName() { return this.form.controls['name'].touched && this.form.controls['name'].invalid; }

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

    const response = await this._service.UpdateAsyc(this.record.id, this.form.value, this.endpoint);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
    this.form.enable();
    if (!response.success) window.location.reload();
  }
}
