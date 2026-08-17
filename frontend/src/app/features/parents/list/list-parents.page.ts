import { Component, inject, signal } from '@angular/core';
import { PagedResult } from '../../../core/api/api.types';
import { AssignedParentResponse } from '../responses/assigned-parent.response';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ParentService } from '../services/parent.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { AccordeonComponent } from '../../../core/ui/accordeon/accordeon.component';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { AlertType } from '../../../core/utils/alert.type';

@Component({
  selector: 'app-list-parents.page',
  imports: [
    AccordeonComponent,
    ReactiveFormsModule,
    UiInputComponent,
    RouterLink
  ],
  templateUrl: './list-parents.page.html',
  styleUrl: './list-parents.page.scss',
})
export class ListParentsPage {
  private readonly _form = inject(FormBuilder);
  private readonly _service = inject(ParentService);
  private readonly _alert = inject(AlertService);
  private readonly _me = inject(ActivatedRoute);

  readonly page = signal<PagedResult<AssignedParentResponse>>({
    items: [],
    totalCount: 0,
    page: 1,
    size: 0,
    totalPages: 0,
    hasPrevious: false,
    hasNext: false
  });
  readonly form = this._form.nonNullable.group({
    name: ['', Validators.maxLength(80)]
  });

  async changePageAsync(
    isNext: boolean
  ) {
    const page = this.page();
    page.page = page.page + (isNext ? 1 : -1);
    await this.loadAsync();
  }

  async loadAsync() {
    if (this._alert.loading()) return;
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    this._alert.startLoading();
    const raw = this.form.getRawValue();
    const page = this.page().page;
    const parentName = raw.name === null || raw.name.trim() === ''
      ? null : raw.name.trim();

    const hasAlert = (this._me.snapshot.data['alert'] as AlertType[] | undefined)
      ?.includes(AlertType.LONELY_PARENTS) ?? false;
    const response = !hasAlert
      ? await this._service.toListAsync(page, parentName)
      : await this._service.lonelyToListAsync(page, parentName);

    this._alert.clear();

    if (!response.isSuccess || response.data === undefined) {
      this._alert.error(response.message);
      return;
    }
    this.page.set(response.data);
  }

  invalidName() {
    const control = this.form.get('name');
    if (!control) return null;
    if (!control.touched || control.valid) return null;
    return 'El nombre del padre no puede exceder los 80 caracteres';
  }
}
