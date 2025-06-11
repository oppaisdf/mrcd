import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ActionsRoutingModule } from './actions-routing.module';
import { NewComponent } from './routes/new/new.component';
import { AllComponent } from './routes/all/all.component';
import { ActionService } from './services/action.service';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    NewComponent,
    AllComponent
  ],
  imports: [
    CommonModule,
    ActionsRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    ActionService
  ]
})
export class ActionsModule { }
