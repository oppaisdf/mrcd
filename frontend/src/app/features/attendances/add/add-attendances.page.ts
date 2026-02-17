import { Component, inject, Query, signal, ViewChild } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { AccordeonComponent } from "../../../core/ui/accordeon/accordeon.component";
import { UiSelectComponent } from "../../../core/ui/select/ui-select.component";
import { UiInputComponent } from "../../../core/ui/input/ui-input.component";
import { AttendancesScannerComponent } from "../scanner/attendances-scanner.component";
import { AlertService } from '../../../shared/alerts/services/alert.service';
import { AttendanceRequest } from '../requests/attendance.request';
import { AttendanceService } from '../services/attendance.service';

@Component({
  selector: 'app-add-attendances.page',
  imports: [
    AccordeonComponent,
    ReactiveFormsModule,
    UiSelectComponent,
    UiInputComponent,
    AttendancesScannerComponent
  ],
  templateUrl: './add-attendances.page.html',
  styleUrl: './add-attendances.page.scss',
})
export class AddAttendancesPage {
  private readonly _form = inject(FormBuilder);
  private readonly _alert = inject(AlertService);
  private readonly _service = inject(AttendanceService);
  @ViewChild(AttendancesScannerComponent) private _scanner!: AttendancesScannerComponent;
  readonly form = this._form.group({
    isAttendance: [true],
    closeToScann: [true],
    date: [new Date()]
  });
  readonly openScanner = signal<boolean>(false);

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

  async scanAsync(
    qr: string
  ) {
    if (this._alert.loading()) return;
    this._scanner.pause();
    this._alert.startLoading();

    const form = this.form.getRawValue();
    const request: AttendanceRequest = {
      personId: qr,
      isAttendance: form.isAttendance ?? true,
      date: form.date === null ? undefined : form.date
    };
    const response = await this._service.addAsync(request);
    this._alert.clear();

    if (!response.isSuccess)
      this._alert.error(response.message);
    if (form.closeToScann) {
      this._scanner.stop();
      this.openScanner.set(false);
    } else await this._scanner.startScannAsync();
  }

  async startScanAsync() {
    const openScanner = this.openScanner();
    this.openScanner.set(!openScanner);
    if (!openScanner) await this._scanner.startScannAsync();
    else this._scanner.stop();
  }
}
