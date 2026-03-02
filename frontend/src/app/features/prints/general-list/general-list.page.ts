import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { PersonService } from '../../people/services/person.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { GeneralListResponse } from '../../people/responses/general-list.response';
import { AccordeonComponent } from '../../../core/ui/accordeon/accordeon.component';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { UiSelectComponent } from '../../../core/ui/select/ui-select.component';
import { UiPrintComponent } from '../../../core/ui/print/ui-print.component';
import { SimpleParentResposne } from '../../parents/responses/simple-parent.response';

@Component({
  selector: 'app-general-list.page',
  imports: [
    AccordeonComponent,
    ReactiveFormsModule,
    UiSelectComponent,
    UiPrintComponent
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

  items(
    controlName: keyof typeof this.form.controls
  ) {
    var labelA = ({
      "showDOB": "Mostrar fecha de nacimiento",
      "showGodparents": "Mostrar padrinos",
      "showParents": "Mostrar padres",
      "showPhones": "Mostrar teléfonos",
      "showParish": "Mostrar parroquia de bautizo",
      "orderByName": "Order por nombre"
    } as const)[controlName];
    var labelB = ({
      "showDOB": "Ocultar fecha de nacimiento",
      "showGodparents": "Ocultar padrinos",
      "showParents": "Ocultar padres",
      "showPhones": "Ocultar teléfonos",
      "showParish": "Ocultar parroquia de bautizo",
      "orderByName": "Order por fecha de inscripción"
    } as const)[controlName];
    return [{
      label: labelA,
      value: true
    }, {
      label: labelB,
      value: false
    }];
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
    return `${dob.getDate()} de ${dob.toLocaleString('es-ES', { month: 'long' })} del ${dob.getFullYear()}`;
  }
}
