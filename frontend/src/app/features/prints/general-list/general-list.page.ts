import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { PersonService } from '../../people/services/person.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { GeneralListResponse } from '../../people/responses/general-list.response';
import { AccordeonComponent } from '../../../core/ui/accordeon/accordeon.component';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { UiPrintComponent } from '../../../core/ui/print/ui-print.component';
import { SimpleParentResposne } from '../../parents/responses/simple-parent.response';
import { UiSwitchComponent } from '../../../core/ui/switch/ui-switch.component';

@Component({
  selector: 'app-general-list.page',
  imports: [
    AccordeonComponent,
    ReactiveFormsModule,
    UiPrintComponent,
    UiSwitchComponent
  ],
  templateUrl: './general-list.page.html',
  styleUrl: './general-list.page.scss',
})
export class GeneralListPage implements OnInit {
  private readonly _service = inject(PersonService);
  private readonly _alert = inject(AlertService);
  private readonly _form = inject(FormBuilder);
  private readonly _people = signal<Array<GeneralListResponse>>([]);
  readonly form = this._form.nonNullable.group({
    showDay: [true],
    showGender: [true],
    showDOB: [false],
    showGodparents: [false],
    showParents: [false],
    showPhones: [false],
    showParish: [false],
    orderByName: [true]
  });
  readonly peopleSorted = computed(() => {
    const people = this._people();
    const orderByName = this.form.controls.orderByName.value;
    return people.sort((a, b) => orderByName
      ? a.personName.localeCompare(b.personName, 'es', { sensitivity: 'base' })
      : new Date(a.registrationDate).getTime() - new Date(b.registrationDate).getTime()
    );
  });

  async ngOnInit() {
    this._alert.startLoading();
    const response = await this._service.generalListAsync();
    this._alert.clear();
    if (!response.isSuccess)
      this._alert.error(response.message);
    else
      this._people.set(response.data ?? []);
  }

  value(
    controlName: keyof typeof this.form.controls
  ) {
    const control = this.form.get(controlName);
    if (!control) return false;
    return control.value;
  }

  parentsInLine(
    parents: Array<SimpleParentResposne>
  ) {
    const showPhone = this.value('showPhones');
    return parents.reduce((a, b) =>
      a += `${a.length > 0 ? ', ' : ''}${b.name}${b.phone && showPhone ? `(${b.phone})` : ''}`,
      ''
    );
  }

  formatedDOB(
    date: Date
  ) {
    const dob = new Date(date);
    return `${dob.getUTCDate()} de ${dob.toLocaleString('es-ES', {
      month: 'long',
      timeZone: 'UTC'
    })} del ${dob.getUTCFullYear()}`;
  }
}
