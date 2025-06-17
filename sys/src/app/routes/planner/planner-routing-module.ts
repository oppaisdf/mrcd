import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Planner } from './routes/planner';

const routes: Routes = [
  { path: '', component: Planner, pathMatch: 'full' },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlannerRoutingModule { }
