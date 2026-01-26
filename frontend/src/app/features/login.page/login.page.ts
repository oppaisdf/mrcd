import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login.page',
  imports: [ReactiveFormsModule],
  templateUrl: './login.page.html',
  styleUrl: './login.page.scss',
})
export class LoginPage {
  private readonly _form = inject(FormBuilder);
  private readonly _service = inject(AuthService);
  private readonly _router = inject(Router);
  readonly form = this._form.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  readonly isLoading = signal<boolean>(false);
  readonly message = signal<string>('');

  async loginAsync() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    if (this.isLoading()) return;

    this.isLoading.set(true);
    const request = this.form.getRawValue();
    const response = await this._service.loginAsync(request.username, request.password);
    this.isLoading.set(false);
    if (response) this.message.set(response);
    else this._router.navigateByUrl('/');
  }

  invalidField(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.controls[controlName];
    return control.touched && control.invalid;
  }
}
