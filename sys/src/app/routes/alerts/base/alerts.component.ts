import { Component, OnInit } from '@angular/core';
import { AlertService } from '../services/alert.service';
import { AlertResponse } from '../responses/alert';

@Component({
  selector: 'alerts-alerts',
  standalone: false,
  templateUrl: './alerts.component.html',
  styleUrl: './alerts.component.sass'
})
export class AlertsComponent implements OnInit {
  constructor(
    private _service: AlertService
  ) { }

  alerts: AlertResponse[] = [];

  async ngOnInit() {
    const alertIds = [1, 2, 3, 4];
    const alertPromises = alertIds.map(id =>
      this._service.AlertCountAsync(id).then(response => {
        if (response.success) {
          this.alerts.push({
            alert: id,
            description: this.GetDescriptionByAlert(id),
            counter: +response.message,
            route: this.GetRouteByAlert(id)
          });
        }
      })
    );
    await Promise.all(alertPromises);
  }

  private GetDescriptionByAlert(
    alert: number
  ) {
    switch (alert) {
      case 1: return 'Confirmandos con pagos pendientes';
      case 2: return 'Confirmandos sin padrinos';
      case 3: return 'Padres/Padrinos sin hijos/ahijados';
      case 4: return 'Confirmandos con documentos pendientes';
      default: return '';
    }
  }

  private GetRouteByAlert(
    alert: number
  ) {
    switch (alert) {
      case 1: case 2: case 4: return '/person/all';
      case 3: return '/parent/all';
      default: return '';
    }
  }
}
