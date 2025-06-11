import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DocService } from '../../services/doc.service';
import { Router } from '@angular/router';

@Component({
  selector: 'documents-new',
  standalone: false,
  templateUrl: './new.component.html',
  styleUrl: './new.component.sass'
})
export class NewComponent {
  constructor(
    private _form: FormBuilder,
    private _service: DocService,
    private _router: Router
  ) {
    this.form = this._form.group({
      name: ['', [Validators.required, Validators.maxLength(10)]]
    });
  }

  form: FormGroup;
  isAdding = false;
  success = true;
  message = '';

  get InvalidName() { return this.form.controls['name'].invalid && this.form.controls['name'].touched; }

  async AddAsync() {
    if (this.isAdding) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    const response = await this._service.AddAsync(this.form.value);
    this.message = response.message;
    this.success = response.success;
    if (!this.success) return;
    this._router.navigateByUrl('/docs/all');
  }
}
