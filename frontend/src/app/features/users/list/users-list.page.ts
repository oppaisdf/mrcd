import { Component, effect, inject, signal } from '@angular/core';
import { UserService } from '../services/user.service';
import { UserDTO } from '../dtos/UserDTO';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { RouterLink } from "@angular/router";
import { UsersDetailsComponent } from '../details/users-details.component';

@Component({
  selector: 'app-users-list.page',
  imports: [
    RouterLink,
    UsersDetailsComponent
  ],
  templateUrl: './users-list.page.html',
  styleUrl: './users-list.page.scss',
})
export class UsersListPage {
  private readonly _service = inject(UserService);
  private readonly _alert = inject(AlertService);
  readonly selectedUser = signal<UserDTO | undefined>(undefined);
  readonly users = signal<Array<UserDTO>>([]);

  private async loadAsync() {
    if (this._alert.loading()) return;
    const response = await this._service.toListAsync();
    if (!response.isSuccess) {
      this._alert.error(response.message!);
      return;
    }
    this.users.set(response.data ?? []);
  }
  private _ = effect(() => this.loadAsync());

  selectUser(
    user: UserDTO
  ) {
    this.selectedUser.set(user);
  }
}
