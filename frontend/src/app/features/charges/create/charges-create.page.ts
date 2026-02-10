import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { ChargesService } from '../services/charges.service';
import { CreateChargeRequest } from '../requests/create-charge.request';
import { Router } from '@angular/router';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";

@Component({
  selector: 'app-charges-create.page',
  imports: [
    ReactiveFormsModule,
    UiInputComponent
  ],
  templateUrl: './charges-create.page.html',
  styleUrl: './charges-create.page.scss',
})
export class ChargesCreatePage {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(ChargesService);
  private readonly _router = inject(Router);
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.nonNullable.group({
    name: ['', [Validators.maxLength(30), Validators.required]],
    amount: [1, [Validators.required, Validators.max(500), Validators.min(1)]]
  });

  hint(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    if (!control) return null;
    if (!control.touched || control.valid) return null;
    const error = ({
      'name': 'El nombre no puede exceder los 30 caracteres y es requerido',
      'amount': 'El monto debe estar entre 1-500 y es requerido'
    } as const)[controlName];
    return error;
  }

  async createAsync() {
    if (this._alert.loading()) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this._alert.startLoading();

    const raw = this.form.getRawValue();
    const request: CreateChargeRequest = {
      name: raw.name,
      amount: raw.amount
    };
    const response = await this._service.createAsync(request);
    this._alert.clear();

    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success("Se ha registrado el cobro exitosamente");
    this._router.navigateByUrl('/charges');
  }
}
