import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,

  templateUrl: './login.component.html',
  styleUrl: './login.component.sass'
})
export class LoginComponent {
  constructor(
    private _form: FormBuilder,
    private _service: LoginService,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(15)]],
      pass: ['', [Validators.required, Validators.maxLength(15)]]
    });
  }

  form: FormGroup;
  isLogging = false;
  lblMessage = '';
  isSuccess = true;

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  async Login() {
    if (this.isLogging) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isLogging = true;
    this.form.disable();

    const response = await this._service.Login(this.form.value);
    this.lblMessage = response.message;
    this.isSuccess = response.success;

    this.isLogging = false;
    this.form.enable();
    this.form.reset();

    if (response.success) this._router.navigateByUrl('/alerts');
  }
}
