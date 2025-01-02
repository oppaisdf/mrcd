import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ActivatedRoute, TitleStrategy } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserRequest } from '../../models/requests/user';

@Component({
  selector: 'user-detail',
  standalone: false,
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.sass'
})
export class DetailComponent implements OnInit {
  constructor(
    private _service: UserService,
    private _me: ActivatedRoute,
    private _form: FormBuilder
  ) {
    this.form = this._form.group({
      id: [''],
      username: ['', [Validators.required, Validators.maxLength(15)]],
      password: ['', [Validators.maxLength(30), Validators.minLength(5)]],
      isActive: [false]
    });
  }

  form: FormGroup;
  roles: { name: string, active: boolean }[] = [];
  private _roleRequest: string[] = [];
  private _old: UserRequest = {};
  message = '';
  success = true;
  isUpdating = false;

  get isActive() { return this.GetValue('isActive'); }
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
    const id = this._me.snapshot.paramMap.get('id')!;
    const response = await this._service.GetByIdAsync(id);

    if (!response.success) {
      this.message = response.message;
      this.success = response.success;
      return;
    }

    // Update info in form
    this.form.patchValue({
      id: response.data!.id,
      username: response.data!.username,
      isActive: response.data!.isActive
    });
    this._roleRequest = response.data!.roles;
    if (!response.data!.isActive) {
      this.form.disable();
      this.form.controls['isActive'].enable();
    }

    //Save old data
    this._old.username = response.data!.username;
    this._old.isActive = response.data!.isActive;
    this._old.roles = [...response.data!.roles];

    //Load roles
    const rolesResponse = await this._service.GetRolesAsync();
    if (!rolesResponse.success) {
      this.message = rolesResponse.message;
      this.success = rolesResponse.success;
      return;
    }
    this.roles = rolesResponse.data!.map(r => ({
      name: r.name,
      active: false
    }
    ));
    this.roles.forEach(r => r.active = this._roleRequest.findIndex(x => x === r.name) > -1);
  }

  OnSelectRole(
    role: string
  ) {
    if (this.isUpdating) return;
    if (!this.isActive) return;
    const index = this._roleRequest.findIndex(r => r === role);
    if (index < 0) this._roleRequest.push(role);
    else this._roleRequest.splice(index, 1);
  }

  async UpdateAsync() {
    if (this.isUpdating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.isUpdating = true;
    this.form.disable();

    const request: UserRequest = {
      username: this._old.username !== this.GetValue('username') ? this.GetValue('username') : undefined,
      password: this.GetValue('password') !== '' ? this.GetValue('password') : undefined,
      roles: this._old.roles!.sort().join(',') !== this._roleRequest.sort().join(',') ? this._roleRequest : undefined,
      isActive: this._old.isActive !== this.GetValue('isActive') ? this.GetValue('isActive') : undefined
    };
    const response = await this._service.UpdateAsync(this.GetValue('id'), request);
    this.message = response.message;
    this.success = response.success;

    this.isUpdating = false;
    if (!response.success) {
      if (this._old.isActive) this.form.enable();
      else this.form.controls['isActive'].enable();
      return;
    }

    this._old.username = this._old.username !== this.GetValue('username') ? this.GetValue('username') : this._old.username;
    this._old.roles = this._old.roles!.sort().join(',') !== this._roleRequest.sort().join(',') ? this._roleRequest : this._old.roles;
    this._old.isActive = this._old.isActive !== this.GetValue('isActive') ? this.GetValue('isActive') : this._old.isActive;
    if (this._old.isActive) this.form.enable();
    else this.form.controls['isActive'].enable();
  }
}
