import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrintsRoutingModule } from './prints-routing.module';
import { BadgeComponent } from './routes/badge/badge.component';
import { PrinterService } from './services/printer.service';
import { QRCodeComponent } from 'angularx-qrcode';
import { ListComponent } from './routes/list/list.component';
import { PrinterComponent } from './components/printer/printer.component'
import { SharedModule } from "../../shared/shared.module";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AttendanceComponent } from './routes/attendance/attendance.component';
import { Planner } from './routes/planner/planner';
import { Certificates } from './routes/certificates/certificates';

@NgModule({
  declarations: [
    BadgeComponent,
    ListComponent,
    PrinterComponent,
    AttendanceComponent,
    Planner,
    Certificates
  ],
  imports: [
    CommonModule,
    PrintsRoutingModule,
    QRCodeComponent,
    SharedModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    PrinterService
  ]
})
export class PrintsModule { }
