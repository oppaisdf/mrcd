import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DegreeService } from '../../services/degree.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new',
  standalone: false,

  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent {
  constructor(
    private _form: FormBuilder,
    private _service: DegreeService,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(10)]]
    });
  }

  message = '';
  success = true;
  isAdding = false;
  form: FormGroup;

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
    if (response.success) this._router.navigateByUrl('/degree/all');
  }
}
