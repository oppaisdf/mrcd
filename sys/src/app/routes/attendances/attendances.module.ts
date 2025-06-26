import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AttendancesRoutingModule } from './attendances-routing.module';
import { AttendanceService } from './services/attendance.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ManualComponent } from './routes/manual/manual.component';
import { SharedModule } from '../../shared/shared.module';
import { ScannerComponent } from './components/scanner/scanner.component';
import { Attendance } from './routes/attendance/attendance';
import { AllDay } from './routes/all-day/all-day';
import { Del } from './routes/del/del';

@NgModule({
  declarations: [
    ManualComponent,
    ScannerComponent,
    Attendance,
    AllDay,
    Del
  ], imports: [
    CommonModule,
    AttendancesRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule
  ], providers: [
    AttendanceService
  ]
})
export class AttendancesModule { }
