import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { Router } from '@angular/router';
import { RoleResponse } from '../../../roles/responses/role';
import { UserRequest } from '../../models/requests/user';

@Component({
  selector: 'user-new',
  standalone: false,
  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent implements OnInit {
  constructor(
    private _form: FormBuilder,
    private _service: UserService,
    private _router: Router
  ) {
    this.form = this._form.group({
      username: ['', [Validators.required, Validators.maxLength(15)]],
      password: ['', [Validators.required, Validators.maxLength(30), Validators.minLength(5)]]
    });
  }

  form: FormGroup;
  roles: RoleResponse[] = [];
  roleRequest: string[] = [];
  message = '';
  success = true;
  isAdding = false;

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  async ngOnInit() {
    const response = await this._service.GetRolesAsync();
    if (response.success) this.roles = response.data!;
    this.message = response.message;
    this.success = response.success;
  }

  OnCheckRole(
    role: string
  ) {
    const index = this.roleRequest.findIndex(r => r === role);
    if (index < 0)
      this.roleRequest.push(role);
    else
      this.roleRequest.splice(index, 1);
  }

  async AddAsync() {
    if (this.isAdding) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isAdding = true;
    this.form.disable();

    const request: UserRequest = {
      username: this.form.controls['username'].value,
      password: this.form.controls['password'].value,
      roles: this.roleRequest
    };
    const response = await this._service.CreateAsync(request);
    this.message = response.message;
    this.success = response.success;

    this.isAdding = false;
    this.form.enable();
    if (!response.success) return;
    this._router.navigateByUrl(`/user/${response.data!.id}`);
  }
}
