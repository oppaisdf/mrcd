import { Component, computed, inject, signal } from '@angular/core';
import { AttendanceService } from '../../attendances/services/attendance.service';
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { UiSelectComponent } from '../../../core/ui/select/ui-select.component';
import { AttendanceResponse } from '../../attendances/responses/attendance.response';
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { UiPrintComponent } from "../../../core/ui/print/ui-print.component";

@Component({
  selector: 'app-print-attendance.page',
  imports: [
    UiSelectComponent,
    ReactiveFormsModule,
    AccordeonComponent,
    UiInputComponent,
    UiPrintComponent
  ],
  templateUrl: './print-attendance.page.html',
  styleUrl: './print-attendance.page.scss',
})
export class PrintAttendancePage {
  private readonly _service = inject(AttendanceService);
  private readonly _alert = inject(AlertService);
  private readonly _form = inject(FormBuilder);
  readonly form = this._form.nonNullable.group({
    date: [new Date],
    onlyByYear: [true],
    isSunday: [true],
    isMasculine: [true],
    name: ['']
  });
  readonly attendances = signal<Array<AttendanceResponse>>([]);
  readonly headers = computed(() => {
    const attendances = this.attendances();
    if (!attendances) return;
    return attendances[0].dates.map(d => `${d.date.getDate()}-${d.date.getMonth() + 1}`);
  });

  async loadAsync() {
    if (this._alert.loading()) return;
    this._alert.startLoading();
    const params = this.form.getRawValue();
    const response = await this._service.toListAsync(
      params.date,
      params.onlyByYear,
      params.isSunday,
      params.isMasculine,
      params.name.trim().length > 0 ? params.name.trim() : undefined
    );
    this._alert.clear();
    if (!response.isSuccess)
      this._alert.error(response.message);
    else this.attendances.set(response.data ?? []);
  }

  items(
    controlName: 'onlyByYear' | 'isSunday' | 'isMasculine'
  ) {
    const labelA = ({
      'onlyByYear': 'Solo por año',
      'isSunday': 'Domingo',
      'isMasculine': 'Masculino'
    } as const)[controlName];
    const labelB = ({
      'onlyByYear': 'Filtrado por fecha',
      'isSunday': 'Sábado',
      'isMasculine': 'Femenino'
    } as const)[controlName];
    return [{
      label: labelA,
      value: true
    }, {
      label: labelB,
      value: false
    }];
  }
}
