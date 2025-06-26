import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManualComponent } from './routes/manual/manual.component';
import { Attendance } from './routes/attendance/attendance';
import { AllDay } from './routes/all-day/all-day';
import { Del } from './routes/del/del';

const routes: Routes = [
  { path: '', component: Attendance, pathMatch: 'full' },
  { path: 'manual', component: ManualComponent },
  { path: 'all', component: AllDay },
  { path: 'delete', component: Del },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttendancesRoutingModule { }
