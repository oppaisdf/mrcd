import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AttendancesRoutingModule } from './attendances-routing.module';
import { AttendanceService } from './services/attendance.service';
import { AllComponent } from './routes/all/all.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ScanComponent } from './components/scan/scan.component';
import { ManualComponent } from './routes/manual/manual.component';
import { ScanAllComponent } from './components/scan-all/scan-all.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    ScanComponent,
    AllComponent,
    ManualComponent,
    ScanAllComponent
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
