import { Component, inject, input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AlertService } from '../../alerts/services/alert.service';
import { BaseEntitiesService } from '../services/base-entities.service';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { ActivatedRoute, Router } from '@angular/router';
import { BaseEntityRequest } from '../requests/BaseEntity.request';

@Component({
  selector: 'shared-be-create',
  imports: [ReactiveFormsModule, UiInputComponent],
  templateUrl: './base-entities-create.page.html',
  styleUrl: './base-entities-create.page.scss',
})
export class BaseEntitiesCreatePage {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(BaseEntitiesService);
  private readonly _router = inject(Router);
  private readonly _me = inject(ActivatedRoute);
  readonly form = this._form.nonNullable.group({
    name: ['', Validators.required]
  });

  maxLength = input.required<number>();
  title = input.required<string>();
  endpoint = input.required<string>();

  hint() {
    const control = this.form.controls.name;
    if (!control.touched) return null;
    if (control.valid) return null;
    if (control.value?.length ?? 0 > this.maxLength())
      return `La longitud no puede exceder los ${this.maxLength()} caracteres`;
    return 'Este campo es requerido';
  }

  async createAsync() {
    if (this._alert.loading()) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    this._alert.startLoading();
    this.form.disable();

    const request: BaseEntityRequest = this.form.getRawValue();
    const response = await this._service.createAsync(this.endpoint(), request);

    this._alert.clear();
    this.form.enable();

    if (!response.isSuccess) {
      this._alert.error(response.message!);
      return;
    }
    this._alert.success("Registro creado con éxito");
    this._router.navigate(['..'], { relativeTo: this._me });
  }
}
