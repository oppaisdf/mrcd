import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ScanComponent } from './routes/scan/scan.component';
import { AllComponent } from './routes/all/all.component';

const routes: Routes = [
  { path: '', redirectTo: 'all', pathMatch: 'full' },
  { path: 'all', component: AllComponent },
  { path: 'scan', component: ScanComponent },
  { path: '**', redirectTo: 'all' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AttendancesRoutingModule { }
