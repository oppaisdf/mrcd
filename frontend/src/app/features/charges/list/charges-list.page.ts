import { Component, inject, OnInit, signal } from '@angular/core';
import { ChargesService } from '../services/charges.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { ChargeResponse } from '../responses/charge.response';
import { RouterLink } from "@angular/router";
import { CurrencyEnvironment } from '../../../core/environments/currency.environment';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-charges-list.page',
  imports: [
    RouterLink,
    CurrencyPipe
  ],
  templateUrl: './charges-list.page.html',
  styleUrl: './charges-list.page.scss',
})
export class ChargesListPage implements OnInit {
  private readonly _service = inject(ChargesService);
  private readonly _alert = inject(AlertService);
  readonly charges = signal<Array<ChargeResponse>>([]);
  readonly symbol = CurrencyEnvironment.currencySymbol;

  async ngOnInit() {
    if (this._alert.loading()) return;
    this._alert.startLoading();
    const response = await this._service.toListAsync();
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this.charges.set(response.data ?? []);
  }

  async deleteAsync(
    chargeId: string
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();
    const response = await this._service.deleteAsync(chargeId);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    const charges = this.charges();
    const index = charges.findIndex(c => c.id === chargeId);
    charges.splice(index, 1);
    this.charges.set(charges);
    this._alert.success("Se ha eliminado el cobro exitosamente");
  }
}
