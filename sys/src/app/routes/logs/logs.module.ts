import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LogsRoutingModule } from './logs-routing.module';
import { AllComponent } from './routes/all/all.component';
import { SharedModule } from '../../shared/shared.module';
import { LogService } from './services/log.service';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AllComponent
  ],
  imports: [
    CommonModule,
    LogsRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  providers: [
    LogService
  ]
})
export class LogsModule { }
