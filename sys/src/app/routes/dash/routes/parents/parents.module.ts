import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ParentsRoutingModule } from './parents-routing.module';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { DetailComponent } from './routes/detail/detail.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../shared/shared.module';
import { ParentService } from './services/parent.service';

@NgModule({
  declarations: [
    AllComponent,
    NewComponent,
    DetailComponent
  ],
  imports: [
    CommonModule,
    ParentsRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    ParentService
  ]
})
export class ParentsModule { }
