import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DegreesRoutingModule } from './degrees-routing.module';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { ReactiveFormsModule } from '@angular/forms';
import { DegreeService } from './services/degree.service';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    AllComponent,
    NewComponent
  ],
  imports: [
    CommonModule,
    DegreesRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    DegreeService
  ]
})
export class DegreesModule { }
