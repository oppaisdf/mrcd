import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChargesRoutingModule } from './charges-routing.module';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../shared/shared.module';
import { ChargeService } from './services/charge.service';
import { UpdaterComponent } from './components/updater/updater.component';

@NgModule({
  declarations: [
    AllComponent,
    NewComponent,
    UpdaterComponent
  ],
  imports: [
    CommonModule,
    ChargesRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    ChargeService
  ]
})
export class ChargesModule { }
