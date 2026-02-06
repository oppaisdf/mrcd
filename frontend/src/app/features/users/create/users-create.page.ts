import { Component, inject } from '@angular/core';
import { UserFormComponent } from "../form/user-form.component";
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { CreateUserRequest } from '../requests/create-user.request';
import { UpdateUserRequest } from '../requests/update-user.request';

@Component({
  selector: 'app-users-create.page',
  imports: [UserFormComponent],
  templateUrl: './users-create.page.html',
  styleUrl: './users-create.page.scss',
})
export class UsersCreatePage {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(UserService);
  private readonly _router = inject(Router);

  user: UpdateUserRequest = {};

  async createAsync(
    rawUser: UpdateUserRequest
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const request: CreateUserRequest = {
      username: rawUser.username ?? '',
      password: rawUser.password ?? ''
    };
    const response = await this._service.createAsync(request);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message!);
      return;
    }
    this._alert.success('El usuario ha sido creado exitosamente');
    this._router.navigateByUrl('/users');
  }
}
