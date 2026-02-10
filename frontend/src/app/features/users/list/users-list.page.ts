import { Component, inject, OnInit, signal } from '@angular/core';
import { UserService } from '../services/user.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { RouterLink } from "@angular/router";
import { UsersDetailsComponent } from '../details/users-details.component';
import { UserResponse } from '../responses/User.response';

@Component({
  selector: 'app-users-list.page',
  imports: [
    RouterLink,
    UsersDetailsComponent
  ],
  templateUrl: './users-list.page.html',
  styleUrl: './users-list.page.scss',
})
export class UsersListPage implements OnInit {
  private readonly _service = inject(UserService);
  private readonly _alert = inject(AlertService);
  private readonly _selectedUser = signal<UserResponse | null>(null);
  readonly users = signal<Array<UserResponse>>([]);

  async ngOnInit() {
    if (this._alert.loading()) return;
    const response = await this._service.toListAsync();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this.users.set(response.data ?? []);
  }

  selectUser(
    user: UserResponse
  ) {
    this._selectedUser.set(user);
  }

  get selectedUser() { return this._selectedUser(); }
  set selectedUser(user: UserResponse | null) { this._selectedUser.set(user); }
}
