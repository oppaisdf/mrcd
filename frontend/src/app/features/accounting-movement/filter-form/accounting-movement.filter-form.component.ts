import { Component, inject, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { AccountingMovementService } from '../services/accounting-movement.service';
import { AccountingMovementResponse } from '../responses/accounting-movement.response';
import { CurrencyEnvironment } from '../../../core/environments/currency.environment';
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { AccordeonComponent } from '../../../core/ui/accordeon/accordeon.component';
import { UiSelectComponent } from '../../../core/ui/select/ui-select.component';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';

@Component({
  selector: 'filters-accounting-movement',
  imports: [
    AccordeonComponent,
    ReactiveFormsModule,
    UiSelectComponent,
    UiInputComponent
],
  templateUrl: './accounting-movement.filter-form.component.html',
  styleUrl: './accounting-movement.filter-form.component.scss',
})
export class AccountingMovementFilterFormComponent {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(AccountingMovementService);
  readonly form = this._form.nonNullable.group({
    date: [new Date],
    filterOnlyByYear: [false]
  });
  readonly movementsSubmit = output<Array<AccountingMovementResponse>>();
  readonly currencySymbol = CurrencyEnvironment.currencySymbol;

  readonly filterItems: Array<SelectItem<boolean>> = [
    {
      label: 'Filtrar por año',
      value: true
    }, {
      label: 'Filtrar por mes',
      value: false
    }
  ];

  async sendData(){
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const filters = this.form.getRawValue();
    const response = await this._service.toListAsync(filters.filterOnlyByYear, filters.date);

    this._alert.clear();
    if (!this._alert.success){
      this._alert.error(response.message);
      return;
    }
    this.movementsSubmit.emit(response.data!);
  }
}
