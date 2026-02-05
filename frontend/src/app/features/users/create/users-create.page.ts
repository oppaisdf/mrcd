import { Component, inject } from '@angular/core';
import { UserFormComponent } from "../form/user-form.component";
import { UserVM } from '../vms/UserVM';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { UserService } from '../services/user.service';
import { UserRequest } from '../dtos/UserRequest';
import { Router } from '@angular/router';

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

  user: UserVM = {
    username: '',
    password: null
  }

  async createAsync(
    rawUser: UserVM
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const request: UserRequest = {
      username: rawUser.username,
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
