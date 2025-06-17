import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlannerRoutingModule } from './planner-routing-module';
import { Planner } from './routes/planner';
import { Calendar } from './components/calendar/calendar';
import { FormsModule } from '@angular/forms';
import { CalendarService } from './services/calendar.service';
import { Activities } from './components/activities/activities';


@NgModule({
  declarations: [
    Planner,
    Calendar,
    Activities
  ],
  imports: [
    CommonModule,
    PlannerRoutingModule,
    FormsModule
  ],
  providers: [
    CalendarService
  ]
})
export class PlannerModule { }
