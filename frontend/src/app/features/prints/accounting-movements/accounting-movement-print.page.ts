import { CurrencyPipe } from '@angular/common';
import { Component, computed, signal } from '@angular/core';
import { AccountingMovementFilterFormComponent } from '../../accounting-movement/filter-form/accounting-movement.filter-form.component';
import { AccountingMovementResponse } from '../../accounting-movement/responses/accounting-movement.response';
import { UiPrintComponent } from '../../../core/ui/print/ui-print.component';
import { CurrencyEnvironment } from '../../../core/environments/currency.environment';

@Component({
  selector: 'app-accounting-movement-print.page',
  imports: [AccountingMovementFilterFormComponent, UiPrintComponent, CurrencyPipe],
  templateUrl: './accounting-movement-print.page.html',
  styleUrl: './accounting-movement-print.page.scss',
})
export class AccountingMovementPrintPage {
  readonly months = signal<Array<AccountingMovementResponse>>([]);
  readonly currencySymbol = CurrencyEnvironment.currencySymbol;
  readonly period = signal('');
  readonly searched = signal(false);
  readonly ledger = computed(() => this.months().map(month => {
    const movements = [...month.movements].sort((a, b) => a.day - b.day);
    const positive = movements.filter(movement => movement.amount >= 0);
    const negative = movements.filter(movement => movement.amount < 0);
    const incoming = positive.reduce((total, movement) => total + Math.round(movement.amount * 100), 0);
    const outgoing = negative.reduce((total, movement) => total + Math.round(-movement.amount * 100), 0);
    return {
      month: month.month,
      rows: Array.from({ length: Math.max(positive.length, negative.length) }, (_, index) => ({
        positive: positive[index],
        negative: negative[index],
      })),
      incoming, outgoing,
    };
  }));
  readonly totals = computed(() => this.ledger().reduce((total, month) => ({
    incoming: total.incoming + month.incoming,
    outgoing: total.outgoing + month.outgoing,
    count: total.count + month.rows.reduce((count, row) => count + Number(!!row.positive) + Number(!!row.negative), 0),
  }), { incoming: 0, outgoing: 0, count: 0 }));

  toList(months: Array<AccountingMovementResponse>, filters: { date: Date | string; filterOnlyByYear: boolean }) {
    const date = typeof filters.date === 'string'
      ? new Date(`${filters.date.slice(0, 10)}T12:00:00`) : filters.date;
    this.period.set(Number.isNaN(date.getTime()) ? 'Período consultado' :
      new Intl.DateTimeFormat('es', filters.filterOnlyByYear
        ? { year: 'numeric' } : { month: 'long', year: 'numeric' }).format(date));
    this.months.set(months);
    this.searched.set(true);
  }
}
