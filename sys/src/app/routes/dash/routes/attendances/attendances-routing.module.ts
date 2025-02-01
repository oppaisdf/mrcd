import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllComponent } from './routes/all/all.component';
import { ManualComponent } from './routes/manual/manual.component';

const routes: Routes = [
  { path: '', component: AllComponent, pathMatch: 'full' },
  { path: 'manual', component: ManualComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttendancesRoutingModule { }
