import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrintsRoutingModule } from './prints-routing.module';
import { BadgeComponent } from './routes/badge/badge.component';
import { PrinterService } from './services/printer.service';

@NgModule({
  declarations: [
    BadgeComponent
  ],
  imports: [
    CommonModule,
    PrintsRoutingModule
  ],
  providers: [
    PrinterService
  ]
})
export class PrintsModule { }
