import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PeopleRoutingModule } from './people-routing.module';
import { NewComponent } from './routes/new/new.component';
import { AllComponent } from './routes/all/all.component';
import { DetailsComponent } from './routes/details/details.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../../../../shared/shared.module';
import { PersonService } from './services/person.service';
import { ParentsComponent } from './components/parents/parents.component';
import { InParentComponent } from './components/in-parent/in-parent.component';
import { ParentService } from './services/parent.service';
import { ChargeService } from './services/charge.service';
import { ChargesComponent } from './components/charges/charges.component';

@NgModule({
  declarations: [
    NewComponent,
    AllComponent,
    DetailsComponent,
    ParentsComponent,
    InParentComponent,
    ChargesComponent
  ],
  imports: [
    CommonModule,
    PeopleRoutingModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    PersonService,
    ParentService,
    ChargeService
  ]
})
export class PeopleModule { }
