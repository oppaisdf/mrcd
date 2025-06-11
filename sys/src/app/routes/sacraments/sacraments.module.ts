import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SacramentsRoutingModule } from './sacraments-routing.module';
import { NewComponent } from './routes/new/new.component';
import { AllComponent } from './routes/all/all.component';
import { SacramentService } from './services/sacrament.service';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    NewComponent,
    AllComponent
  ],
  imports: [
    CommonModule,
    SacramentsRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    SacramentService
  ]
})
export class SacramentsModule { }
