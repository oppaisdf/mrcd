import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";

@Component({
  selector: 'app-add-attendances.page',
  imports: [
    AccordeonComponent,
    ReactiveFormsModule,
    UiSelectComponent,
    UiInputComponent
  ],
  templateUrl: './add-attendances.page.html',
  styleUrl: './add-attendances.page.scss',
})
export class AddAttendancesPage {
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.nonNullable.group({
    isAttendance: [true],
    closeToScann: [true],
    date: [new Date()]
  });

  items(
    controlName: 'isAttendance' | 'closeToScann'
  ) {
    const labelA = ({
      'isAttendance': 'Asistencia',
      'closeToScann': 'Cerrar al escanear'
    } as const)[controlName];
    const labelB = ({
      'isAttendance': 'Permiso inasistencia',
      'closeToScann': 'Mantener visible al escanear'
    } as const)[controlName];
    return [{
      label: labelA,
      value: true
    }, {
      label: labelB,
      value: false
    }];
  }

  limitDate(
    isMax: boolean
  ) {
    const now = new Date();
    now.setMonth(isMax ? 11 : 0);
    now.setDate(isMax ? 31 : 1);
    return now.toISOString().split('T')[0]
  }
}
