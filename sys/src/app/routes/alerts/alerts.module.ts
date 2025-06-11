import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AlertsRoutingModule } from './alerts-routing.module';
import { AlertsComponent } from './base/alerts.component';
import { AlertService } from './services/alert.service';

@NgModule({
  declarations: [
    AlertsComponent
  ],
  imports: [
    CommonModule,
    AlertsRoutingModule
  ],
  providers: [
    AlertService
  ]
})
export class AlertsModule { }
