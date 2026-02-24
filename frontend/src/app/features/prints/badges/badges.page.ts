import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { UiPrintComponent } from '../../../core/ui/print/ui-print.component';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PersonService } from '../../people/services/person.service';
import { PersonBadgeVM } from '../../people/vms/person-badge.vm';
import { QRCodeComponent } from 'angularx-qrcode';
import { FormBuilder, ɵInternalFormsSharedModule, ReactiveFormsModule } from '@angular/forms';
import { SelectItem } from '../../../core/ui/select/SelectItem';
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { toSignal } from '@angular/core/rxjs-interop';
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";

@Component({
  selector: 'app-badges.page',
  imports: [
    UiPrintComponent,
    QRCodeComponent,
    UiSelectComponent,
    ɵInternalFormsSharedModule,
    ReactiveFormsModule,
    AccordeonComponent
  ],
  templateUrl: './badges.page.html',
  styleUrl: './badges.page.scss',
})
export class BadgesPage implements OnInit {
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(PersonService);
  private readonly _people = signal<Array<PersonBadgeVM>>([]);
  private readonly _printablePeople = signal<Array<PersonBadgeVM>>([]);
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.nonNullable.group({
    columns: [2],
    orderByName: [true]
  });

  get cols() { return this.form.controls.columns.value; }
  readonly columns: Array<SelectItem<number>> = [
    {
      label: '2',
      value: 2
    }, {
      label: '3',
      value: 3
    }, {
      label: '4',
      value: 4
    }
  ];
  readonly order: Array<SelectItem<boolean>> = [{
    label: 'Por nombre',
    value: true
  }, {
    label: 'Por fecha de inscripción',
    value: false
  }];

  async ngOnInit() {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = await this._service.generalListAsync();
    this._alert.clear();
    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._people.set(response.data?.map(p => ({
      personId: p.personId,
      personName: p.personName,
      registrationDate: p.registrationDate,
      isSunday: p.isSunday,
      isSelected: false
    })) ?? []);
  }

  selectPerson(
    personId: string
  ) {
    const people = this._people();
    const person = people.find(p => p.personId === personId);
    if (!person) return;
    const printables = [...this._printablePeople()];
    const printableIndex = printables.findIndex(p => p.personId === personId);

    if (printableIndex !== -1) {
      person.isSelected = false;
      printables.splice(printableIndex, 1);
      this._people.set(people);
      this._printablePeople.set(printables);
      return;
    }
    printables.push(person);
    person.isSelected = true;
    this._printablePeople.set(printables);
    this._people.set(people);
  }

  orderByNameSign = toSignal(
    this.form.controls.orderByName.valueChanges,
    { initialValue: this.form.controls.orderByName.value }
  );

  peopleSorted = computed(() => {
    const orderByName = this.orderByNameSign();
    return this._people().sort((a, b) => orderByName
      ? a.personName.localeCompare(b.personName, 'es', { sensitivity: 'base' })
      : new Date(a.registrationDate).getTime() - new Date(b.registrationDate).getTime()
    );
  });

  printablesSorted = computed(() => {
    const orderByName = this.orderByNameSign();
    return [... this._printablePeople()].sort((a, b) => orderByName
      ? a.personName.localeCompare(b.personName, 'es', { sensitivity: 'base' })
      : new Date(a.registrationDate).getTime() - new Date(b.registrationDate).getTime()
    );
  });
}
