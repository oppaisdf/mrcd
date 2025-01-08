import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrintsRoutingModule } from './prints-routing.module';
import { BadgeComponent } from './routes/badge/badge.component';
import { PrinterService } from './services/printer.service';
import { QRCodeComponent } from 'angularx-qrcode';
import { ListComponent } from './routes/list/list.component';
import { PrinterComponent } from './components/printer/printer.component'
import { SharedModule } from "../../../../shared/shared.module";
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    BadgeComponent,
    ListComponent,
    PrinterComponent
  ],
  imports: [
    CommonModule,
    PrintsRoutingModule,
    QRCodeComponent,
    SharedModule,
    FormsModule
  ],
  providers: [
    PrinterService
  ]
})
export class PrintsModule { }
