import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BadgeComponent } from './routes/badge/badge.component';
import { admGuard } from '../../core/guards/adm.guard';
import { ListComponent } from './routes/list/list.component';
import { AttendanceComponent } from './routes/attendance/attendance.component';
import { Planner } from './routes/planner/planner';
import { Certificates } from './routes/certificates/certificates';

const routes: Routes = [
  { path: '', redirectTo: 'list', pathMatch: 'full' },
  { path: 'badge', component: BadgeComponent, canActivate: [admGuard] },
  { path: 'list', component: ListComponent },
  { path: 'attendance', component: AttendanceComponent },
  { path: 'planner', component: Planner },
  { path: 'certificate', component: Certificates },
  { path: '**', redirectTo: 'list' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PrintsRoutingModule { }
