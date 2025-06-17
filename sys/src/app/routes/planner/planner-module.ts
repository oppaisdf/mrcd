import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlannerRoutingModule } from './planner-routing-module';
import { Planner } from './routes/core/planner';
import { Calendar } from './components/calendar/calendar';
import { FormsModule } from '@angular/forms';
import { CalendarService } from './services/calendar.service';
import { Activities } from './components/activities/activities';
import { Activity } from './routes/activity/activity';


@NgModule({
  declarations: [
    Planner,
    Calendar,
    Activities,
    Activity
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
