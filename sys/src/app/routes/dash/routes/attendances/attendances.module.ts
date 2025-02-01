import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AttendancesRoutingModule } from './attendances-routing.module';
import { AttendanceService } from './services/attendance.service';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { SharedModule } from '../../../../shared/shared.module';
import { AllComponent } from './routes/all/all.component';
import { FormsModule } from '@angular/forms';
import { ScanComponent } from './components/scan/scan.component';
import { ManualComponent } from './routes/manual/manual.component';

@NgModule({
  declarations: [
    ScanComponent,
    AllComponent,
    ManualComponent
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
