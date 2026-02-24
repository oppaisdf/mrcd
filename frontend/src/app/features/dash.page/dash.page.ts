import { Component, inject, OnInit, signal } from '@angular/core';
import { ApiService } from '../../core/api/api.service';
import { UserFormComponent } from "../users/form/user-form.component";
import { UserVM } from '../users/vms/User.vm';
import { UserResponse } from '../users/responses/User.response';
import { UsedRoleResponse } from '../roles/responses/UsedRole.response';
import { UpdateUserRequest } from '../users/requests/update-user.request';
import { AlertService } from '../../shared/alerts/services/alert.service';

type AlertResponse = {
  count: number;
  message: string;
};

@Component({
  selector: 'app-dash.page',
  imports: [UserFormComponent],
  templateUrl: './dash.page.html',
  styleUrl: './dash.page.scss',
})
export class DashPage implements OnInit {
  private readonly _api = inject(ApiService);
  private readonly _alertIds = ['0', '1', '2', '3'];
  private readonly _alert = inject(AlertService);
  readonly alerts = signal<Array<AlertResponse>>([]);
  readonly roles = signal<Array<UsedRoleResponse>>([]);
  readonly user = signal<UserVM>({
    username: null,
    password: null
  });

  async ngOnInit() {
    await this.loadMeAsync();
    await this.loadAlertsAsync();
  }

  private async loadAlertsAsync() {
    const alertPromises = this._alertIds.map(id =>
      this._api.getAsync<AlertResponse>(`/alert/${id}`)
        .then(response => {
          if (response.isSuccess) {
            const alerts = this.alerts();
            alerts.push({
              count: response.data?.count ?? 0,
              message: response.data?.message ?? 'unknow'
            });
            this.alerts.set([...alerts]);
          }
        })
    );
    await Promise.all(alertPromises);
  }

  private async loadMeAsync() {
    const response = await this._api.getAsync<UserResponse>('/user/me');
    if (!response.isSuccess) return;
    this.user.set({
      username: response.data?.username ?? '',
      password: ''
    });
    this.roles.set(response.data?.roles.filter(r => r.hasRole) ?? []);
  }

  async updateMeAsync(
    me: UserVM
  ) {
    const user = this.user();
    const request: UpdateUserRequest = {};
    if (me.username && me.username !== user.username)
      request.username = me.username;
    if (me.password && me.password !== '')
      request.password = me.password;

    if (!request.username && !request.password) return;
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = await this._api.patchAsync('/user/me', request);
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success('Se ha actualizado la información correctamente');
    if (!request.username) return;
    user.username = request.username;
    this.user.set(user);
  }
}
