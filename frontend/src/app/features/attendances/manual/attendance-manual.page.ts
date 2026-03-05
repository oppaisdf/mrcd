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
    type: [1]
  });

  readonly people = computed(() => {
    const isSunday = this.form.controls.isSunday.value;
    const people = this._people();
    return people.filter(p => p.isSunday === isSunday);
  });
  readonly year = new Date().getFullYear();

  readonly days = [{
    label: 'Domingo',
    value: true
  }, {
    label: 'Sábado',
    value: false
  }];

  readonly attendanceTypes = [{
    label: 'Asistencia',
    value: 1
  }, {
    label: 'Permiso de inasistencia',
    value: 2
  }, {
    label: 'Eliminar asistencia',
    value: 3
  }];

  get isDelete() {
    const type = this.form.controls.type.value;
    return type === 3;
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
      isAttendance: form.type === 1,
      date: now !== date ? date : undefined
    };
    const response = form.type !== 3
      ? await this._service.addAsync(request)
      : await this._service.delAsync(personId, request.date);
    this._alert.clear();

    if (!response.isSuccess) {
      this._alert.error(response.message);
      return;
    }
    this._alert.success(`Se ha registrado la asistencia correctamente`);
  }
}
