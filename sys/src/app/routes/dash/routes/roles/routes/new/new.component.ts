import { Component } from '@angular/core';
import { RoleService } from '../../services/role.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'role-new',
  standalone: false,

  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent {
  constructor(
    private _service: RoleService,
    private _form: FormBuilder,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(10)]]
    });
  }

  form: FormGroup;
  lblMessage = '';
  isSuccess = true;
  isAdding = false;
  get isInvalidName() { return this.form.controls['name'].touched && this.form.controls['name'].invalid; }

  async AddAsync() {
    if (this.isAdding) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.form.disable();
    this.isAdding = true;

    const response = await this._service.AddAsync(this.form.value);
    this.lblMessage = response.message;
    this.isSuccess = response.success;

    this.isAdding = false;
    this.form.enable();
    if (!response.success) return;
    this.form.reset();
    this._router.navigateByUrl('/role/all');
  }
}
