import { Component, inject, signal } from '@angular/core';
import { PersonService } from '../services/person.service';
import { SimplePersonResponse } from '../responses/simple-person.response';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PagedResult } from '../../../core/api/api.types';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";
import { AlertType } from '../../../core/utils/alert.type';

@Component({
  selector: 'people-list.page',
  imports: [
    ReactiveFormsModule,
    UiInputComponent,
    UiSelectComponent,
    RouterLink,
    AccordeonComponent
  ],
  templateUrl: './people-list.page.html',
  styleUrl: './people-list.page.scss',
})
export class PeopleListPage {
  private readonly _service = inject(PersonService);
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _me = inject(ActivatedRoute);

  readonly form = this._form.group({
    isActive: [true, Validators.required],
    name: [''],
    isMasculine: []
  });
  readonly people = signal<PagedResult<SimplePersonResponse>>({
    items: [],
    totalCount: 0,
    page: 1,
    size: 0,
    totalPages: 0,
    hasPrevious: false,
    hasNext: false
  });

  async getAsync() {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const form = this.form.getRawValue();
    const alertType = (this._me.snapshot.queryParamMap.get('alert') as AlertType | undefined);
    let response;
    switch(alertType){
      case AlertType.PENDING_CHARGES:
        response = await this._service.pendingChargesToListAsync(
          form.isActive ?? true,
          this.people().page,
          form.name ?? undefined,
          form.isMasculine ?? undefined
        );
        break;
      case AlertType.PENDING_DOCUMENTS:
        response = await this._service.pendingDocumentsToListAsync(
          form.isActive ?? true,
          this.people().page,
          form.name ?? undefined,
          form.isMasculine ?? undefined
        );
        break;
      case AlertType.PENDING_GODPARENTS:
        response = await this._service.pendingGodparentsToListAsync(
          form.isActive ?? true,
          this.people().page,
          form.name ?? undefined,
          form.isMasculine ?? undefined
        );
        break;
      default: response = await this._service.toListAsync(
        form.isActive ?? true,
        this.people().page,
        form.name ?? undefined,
        form.isMasculine ?? undefined
      );
      break;
    }
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    if (!response.data) return;
    this.people.set(response.data);
  }

  getItems(
    controlName: keyof typeof this.form.controls
  ) {
    const labelA = ({
      'isActive': 'Activo',
      'isMasculine': 'Masculino',
      'name': ''
    } as const)[controlName];
    const labelB = ({
      'isActive': 'Inactivo',
      'isMasculine': 'Femenino',
      'name': ''
    } as const)[controlName];
    const response: Array<SelectItem<boolean>> = [{
      label: labelA,
      value: true
    }, {
      label: labelB,
      value: false
    }];
    return response;
  }

  async changePageAsync(
    isNext: boolean
  ) {
    const paged = this.people();
    paged.page = paged.page + (isNext ? 1 : -1);
    await this.getAsync();
  }
}
