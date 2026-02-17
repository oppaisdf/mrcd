import { Component, inject, signal } from '@angular/core';
import { ParentService } from '../services/parent.service';
import { ActivatedRoute } from '@angular/router';
import { SimplePersonResponse } from '../../people/responses/simple-person.response';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";

@Component({
  selector: 'parents-details.page',
  imports: [
    ReactiveFormsModule,
    UiInputComponent,
    UiSelectComponent,
    AccordeonComponent
  ],
  templateUrl: './parents-details.page.html',
  styleUrl: './parents-details.page.scss',
})
export class ParentsDetailsPage {
  private readonly _service = inject(ParentService);
  private readonly _me = inject(ActivatedRoute);
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);

  readonly id: string;
  readonly children = signal<Array<SimplePersonResponse>>([]);
  readonly goodchildren = signal<Array<SimplePersonResponse>>([]);
  readonly genders: Array<SelectItem<boolean>> = [{
    label: 'Masculino',
    value: true
  }, {
    label: 'Femenino',
    value: false
  }];
  readonly form = this._form.nonNullable.group({
    name: [''],
    isMasculine: [false],
    phone: ['']
  });

  constructor() {
    this.id = this._me.snapshot.paramMap.get('id')!;
    this.loadAsync();
  }

  async loadAsync() {
    this._alert.startLoading();
    const response = await this._service.getByIdAsync(this.id);
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this.form.patchValue({
      name: response.data!.name,
      isMasculine: response.data!.isMasculine,
      phone: response.data!.phone
    });
    this.children.set(response.data!.children);
    this.goodchildren.set(response.data!.goodchildren);
  }
}
