import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlannerRoutingModule } from './planner-routing-module';
import { Planner } from './routes/core/planner';
import { Calendar } from './components/calendar/calendar';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CalendarService } from './services/calendar.service';
import { Activities } from './components/activities/activities';
import { Activity } from './routes/activity/activity';
import { SharedModule } from "../../shared/shared.module";
import { Stage } from './routes/stage/stage';
import { NewStage } from './components/new-stage/new-stage';
import { StageService } from './services/stage';

@NgModule({
  declarations: [
    Planner,
    Calendar,
    Activities,
    Activity,
    Stage,
    NewStage
  ],
  imports: [
    CommonModule,
    PlannerRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    CalendarService,
    StageService
  ]
})
export class PlannerModule { }
