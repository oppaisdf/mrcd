import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Planner } from './routes/core/planner';
import { Activity } from './routes/activity/activity';
import { detailsGuard } from '../../core/guards/details-guard';

const routes: Routes = [
  { path: '', component: Planner, pathMatch: 'full' },
  { path: ':id', component: Activity, canActivate: [detailsGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlannerRoutingModule { }
