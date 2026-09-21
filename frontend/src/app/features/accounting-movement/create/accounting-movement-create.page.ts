import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UiInputComponent } from '../../../core/ui/input/ui-input.component';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { AccountingMovementService } from '../services/accounting-movement.service';
import { CreateAccountingMovementRequest } from '../requests/create-accounting-movement.request';
import { Router } from '@angular/router';

@Component({
  selector: 'app-accounting-movement-create.page',
  imports: [ReactiveFormsModule, UiInputComponent],
  templateUrl: './accounting-movement-create.page.html',
  styleUrl: './accounting-movement-create.page.scss',
})
export class AccountingMovementCreatePage {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(AccountingMovementService);
  private readonly _router = inject(Router);
  readonly form = this._form.group({
    name: ['', [Validators.required, Validators.maxLength(50)]],
    amount: [0, [Validators.required, Validators.max(5000), Validators.min(-5000)]]
  });

  hint(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    if (!control) return 'Control not found :c';
    if (!control.touched || control.valid) return null;
    switch(controlName){
      case 'name': return 'El nombre es requerido y no puede exceder los 50 caracteres';
      case 'amount': return 'El monto es requerido y no puede ser mayor o menor a 5000';
      default: return null;
    }
  }

  async addAsync(){
    if (this._alert.loading()) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this._alert.startLoading();

    const rawRequest = this.form.getRawValue();
    const request: CreateAccountingMovementRequest = {
      name: rawRequest.name ?? '',
      amount: rawRequest.amount ?? 0
    };
    const response = await this._service.addAsync(request);

    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success('Se ha agregado el movimiento contable correctamente');
    this._router.navigateByUrl('/accounting/movement/list');
  }
}
