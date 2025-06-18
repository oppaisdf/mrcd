import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Planner } from './routes/core/planner';
import { Activity } from './routes/activity/activity';
import { detailsGuard } from '../../core/guards/details-guard';
import { Stage } from './routes/stage/stage';

const routes: Routes = [
  { path: '', component: Planner, pathMatch: 'full' },
  { path: 'stage', component: Stage },
  { path: ':id', component: Activity, canActivate: [detailsGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlannerRoutingModule { }
