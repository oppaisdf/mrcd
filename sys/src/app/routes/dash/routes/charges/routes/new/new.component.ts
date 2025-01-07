import { Component } from '@angular/core';
import { ChargeService } from '../../services/charge.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../environments/environment';
import { Router } from '@angular/router';

@Component({
  selector: 'charge-new',
  standalone: false,
  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent {
  constructor(
    private _service: ChargeService,
    private _form: FormBuilder,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(11)]],
      total: [0, Validators.required]
    });
  }

  form: FormGroup;
  symbolCurrency = environment.currencySymbol;
  creating = false;
  message = '';
  success = true;

  IsInvalidField(
    control: string
  ) {
    return this.form.controls[control].touched && this.form.controls[control].invalid;
  }

  async AddAsync() {
    if (this.creating) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;
    this.creating = true;
    this.form.disable();

    const response = await this._service.CreateAsync(this.form.value);
    this.message = response.message;
    this.success = response.success;

    this.creating = false;
    this.form.enable();
    if (response.success) this._router.navigateByUrl('/charge/all');
  }
}
