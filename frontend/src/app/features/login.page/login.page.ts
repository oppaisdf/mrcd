import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/auth/auth.service';
import { Router } from '@angular/router';
import { UiInputComponent } from "../../core/ui/input/ui-input.component";
import { AlertService } from '../../shared/alerts/services/alert.service';

@Component({
  selector: 'app-login.page',
  imports: [ReactiveFormsModule, UiInputComponent],
  templateUrl: './login.page.html',
  styleUrl: './login.page.scss',
})
export class LoginPage {
  private readonly _form = inject(FormBuilder);
  private readonly _service = inject(AuthService);
  private readonly _router = inject(Router);
  private readonly _alert = inject(AlertService);
  readonly form = this._form.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  readonly isLoading = this._alert.loading;

  async loginAsync() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    if (this.isLoading()) return;

    this._alert.startLoading();
    this.form.disable();
    const request = this.form.getRawValue();
    const response = await this._service.loginAsync(request.username, request.password);
    this.form.enable();
    this._alert.clear();
    if (response) this._alert.error(response);
    else this._router.navigateByUrl('/');
  }

  invalidField(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.controls[controlName];
    const invalid = control.touched && control.invalid;
    if (!invalid) return undefined;
    switch (controlName) {
      case 'username':
        return 'El usuario es requerido';
      case 'password':
        return 'La contraseña es requerida';
    }
  }
}
