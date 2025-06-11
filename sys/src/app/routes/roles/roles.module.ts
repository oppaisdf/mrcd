import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RolesRoutingModule } from './roles-routing.module';
import { NewComponent } from './routes/new/new.component';
import { AllComponent } from './routes/all/all.component';
import { RoleService } from './services/role.service';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    NewComponent,
    AllComponent
  ],
  imports: [
    CommonModule,
    RolesRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  providers: [
    RoleService
  ]
})
export class RolesModule { }
