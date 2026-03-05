import { Component, computed, inject, signal } from '@angular/core';
import { AccordeonComponent } from '../../../core/ui/accordeon/accordeon.component';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { PersonService } from '../../people/services/person.service';
import { GeneralListResponse } from '../../people/responses/general-list.response';
import { AttendanceRequest } from '../requests/attendance.request';
import { AttendanceService } from '../services/attendance.service';
import { UiSelectComponent } from '../../../core/ui/select/ui-select.component';
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";

@Component({
  selector: 'app-attendance-manual.page',
  imports: [
    AccordeonComponent,
    ReactiveFormsModule,
    UiSelectComponent,
    UiInputComponent
  ],
  templateUrl: './attendance-manual.page.html',
  styleUrl: './attendance-manual.page.scss',
})
export class AttendanceManualPage {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _peopleService = inject(PersonService);
  private readonly _service = inject(AttendanceService);
  private readonly _people = signal<Array<GeneralListResponse>>([]);
  readonly form = this._form.nonNullable.group({
    isSunday: [true],
    date: [new Date()],
    isAttendance: [true]
  });

  readonly people = computed(() => {
    const isSunday = this.form.controls.isSunday.value;
    const people = this._people();
    return people.filter(p => p.isSunday === isSunday);
  });
  readonly year = new Date().getFullYear();

  items(
    controlName: 'isSunday' | 'isAttendance'
  ) {
    const labelA = ({
      'isSunday': 'Domingo',
      'isAttendance': 'Asistencia'
    } as const)[controlName];
    const labelB = ({
      'isSunday': 'Sábado',
      'isAttendance': 'Permiso de inasistencia'
    } as const)[controlName];
    return [{
      label: labelA,
      value: true
    }, {
      label: labelB,
      value: false
    }];
  }

  async loadAsync() {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const form = this.form.getRawValue();
    const response = await this._peopleService.generalListAsync();
    this._alert.clear();

    if (!response.isSuccess)
      this._alert.error(response.message);
    else this._people.set(response.data ?? []);
  }

  async attendanceAsync(
    personId: string
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();
    const form = this.form.getRawValue();

    const now = new Date().toISOString().split("T")[0];
    const date = new Date(form.date).toISOString().split("T")[0];
    const request: AttendanceRequest = {
      personId: personId,
      isAttendance: form.isAttendance,
      date: now !== date ? date : undefined
    };
    const response = await this._service.addAsync(request);
    this._alert.clear();

    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success(`Se ha registrado la asistencia correctamente`);
  }
}
