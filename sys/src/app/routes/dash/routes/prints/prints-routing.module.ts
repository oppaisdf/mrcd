import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BadgeComponent } from './routes/badge/badge.component';

const routes: Routes = [
  { path: '', redirectTo: 'badge', pathMatch: 'full' },
  { path: 'badge', component: BadgeComponent },
  { path: '**', redirectTo: 'badge' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PrintsRoutingModule { }
