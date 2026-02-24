import { Component, inject, OnInit, signal } from '@angular/core';
import { ApiService } from '../../core/api/api.service';

type AlertResponse = {
  count: number;
  message: string;
};

@Component({
  selector: 'app-dash.page',
  imports: [],
  templateUrl: './dash.page.html',
  styleUrl: './dash.page.scss',
})
export class DashPage implements OnInit {
  private readonly _api = inject(ApiService);
  private readonly _alertIds = ['0', '1', '2', '3'];
  readonly alerts = signal<Array<AlertResponse>>([]);

  async ngOnInit() {
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
}
