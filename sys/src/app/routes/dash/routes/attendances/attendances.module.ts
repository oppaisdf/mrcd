import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AttendancesRoutingModule } from './attendances-routing.module';
import { ScanComponent } from './routes/scan/scan.component';
import { AttendanceService } from './services/attendance.service';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { SharedModule } from '../../../../shared/shared.module';
import { AllComponent } from './routes/all/all.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ScanComponent,
    AllComponent
  ], imports: [
    CommonModule,
    AttendancesRoutingModule,
    SharedModule,
    ZXingScannerModule,
    FormsModule
  ], providers: [
    AttendanceService
  ]
})
export class AttendancesModule { }
