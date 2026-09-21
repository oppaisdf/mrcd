import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { AccountingMovementFilterFormComponent } from '../filter-form/accounting-movement.filter-form.component';
import { AccountingMovementResponse } from '../responses/accounting-movement.response';
import { CurrencyEnvironment } from '../../../core/environments/currency.environment';
import { SessionStore } from '../../../core/stores/session.store';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { AccountingMovementService } from '../services/accounting-movement.service';

@Component({
  selector: 'app-accounting-movement-list.page',
  imports: [
    ReactiveFormsModule,
    CurrencyPipe,
    AccountingMovementFilterFormComponent
],
  templateUrl: './accounting-movement-list.page.html',
  styleUrl: './accounting-movement-list.page.scss',
})
export class AccountingMovementListPage {
  readonly movements = signal<Array<AccountingMovementResponse>>([]);
  readonly currencySymbol = CurrencyEnvironment.currencySymbol;
  private readonly _session = inject(SessionStore);
  readonly isAdmin = this._session.hasRole('adm');
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(AccountingMovementService);
  getDate(
    date: Date
  ){
    if (!date) return '';
    const dob = typeof date === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(date)
      ? new Date(`${date}T00:00:00`) : new Date(date);
    if (Number.isNaN(dob.getTime())) return '';
    return `${dob.getDate()} de ${dob.toLocaleString('es-ES', { month: 'long' })} del ${dob.getFullYear()}`;
  }

  toList(
    movements: Array<AccountingMovementResponse>
  ){
    this.movements.set(movements);
  }

  async delAsync(
    id: string
  ){
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = await this._service.delAsync(id);
    this._alert.clear();
    if (!response.isSuccess){
      this._alert.error(response.message);
      return;
    }
    this.movements.set([]);
    this._alert.success('Se eliminó el movimiento contable correctamente');
  }
}
