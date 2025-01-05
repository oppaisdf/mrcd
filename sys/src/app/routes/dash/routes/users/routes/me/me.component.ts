import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserRequest } from '../../models/requests/user';

@Component({
  selector: 'user-me',
  standalone: false,
  templateUrl: './me.component.html',
  styleUrl: './me.component.sass'
})
export class MeComponent implements OnInit {
  constructor(
    private _service: UserService,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      username: ['', [Validators.required, Validators.maxLength(15)]],
      password: ['', [
        Validators.maxLength(30),
        Validators.pattern('^(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{5,}$')
      ]]
    });
  }

  form: FormGroup;
  roles: string[] = [];
  message = '';
  success = true;
  isUpdating = false;
  private _oldName = '';

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  private GetValue(
    control: string
  ) {
    return this.form.controls[control].value;
  }

  async ngOnInit() {
    const response = await this._service.GetByIdAsync("me");
    this.message = response.message;
    this.success = response.success;
    if (!response.success) return;
    this.form.controls['username'].setValue(response.data!.username);
    this._oldName = response.data!.username;
    this.roles = response.data!.roles;
  }

  async UpdateAsync() {
    if (this.isUpdating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isUpdating = true;
    this.form.disable();

    const request: UserRequest = {};
    if (this.GetValue('username') != this._oldName) request.username = this.GetValue('username');
    if (this.GetValue('password')) request.password = this.GetValue('password');
    if (request.username === undefined && request.password === undefined) {
      this.message = 'Nada para actualizar';
      this.isUpdating = false;
      this.form.enable();
      return;
    }
    const response = await this._service.UpdateAsync("me", request);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
    this.form.enable();
    if (response.success) this._oldName = this.GetValue('username');
  }
}
