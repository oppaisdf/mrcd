import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DefaultRequest } from '../../../../core/models/requests/Request';
import { StageService } from '../../services/stage';
import { Router } from '@angular/router';

@Component({
  selector: 'planner-comp-new-stage',
  standalone: false,
  templateUrl: './new-stage.html',
  styleUrl: './new-stage.sass'
})
export class NewStage {
  constructor(
    private _form: FormBuilder,
    private _service: StageService,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(50)]]
    });
  }

  @Input() showModal!: boolean;
  form: FormGroup;
  loading = false;
  message = '';
  success = false;

  InvalidField(
    name: string
  ) {
    const control = this.form.controls[name];
    return control.touched && control.invalid;
  }

  HideModal() {
    if (this.loading) return;
    this.showModal = false;
  }

  private GetValue(
    name: string
  ) {
    return `${this.form.controls[name].value}`.trim();
  }

  async AddStageAsync() {
    if (this.loading) return;
    const name = this.GetValue('name');
    if (name === '') this.form.patchValue({ name: '' });
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    this.loading = true;
    const request: DefaultRequest = {
      name: name
    };
    const response = await this._service.AddStageAsync(request);
    this.message = response.message;
    this.success = response.success
    this.loading = false;

    if (!response.success) return;
    this._router.navigateByUrl('/planner/stage');
  }
}
