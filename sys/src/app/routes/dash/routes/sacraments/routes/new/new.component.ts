import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SacramentService } from '../../services/sacrament.service';
import { Router } from '@angular/router';

@Component({
  selector: 'sacrament-new',
  standalone: false,

  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent {
  constructor(
    private _form: FormBuilder,
    private _service: SacramentService,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(10)]]
    });
  }

  form: FormGroup;
  message = '';
  success = true;
  isAdding = false;

  get isInvalidName() { return this.form.controls['name'].touched && this.form.controls['name'].invalid; }

  async AddAsync() {
    if (this.isAdding) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isAdding = true;
    this.form.disable();

    const response = await this._service.AddAsync(this.form.value);
    this.message = response.message;
    this.success = response.success;

    this.isAdding = false;
    this.form.enable();
    if (response.success) this._router.navigateByUrl('/sacrament/all');
  }
}
