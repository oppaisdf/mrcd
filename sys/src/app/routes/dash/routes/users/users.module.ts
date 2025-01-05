import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersRoutingModule } from './users-routing.module';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { DetailComponent } from './routes/detail/detail.component';
import { SharedModule } from '../../../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { UserService } from './services/user.service';
import { MeComponent } from './routes/me/me.component';

@NgModule({
  declarations: [
    AllComponent,
    NewComponent,
    DetailComponent,
    MeComponent
  ],
  imports: [
    CommonModule,
    UsersRoutingModule,
    SharedModule,
    ReactiveFormsModule
  ],
  providers: [
    UserService
  ]
})
export class UsersModule { }
