import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BadgeComponent } from './routes/badge/badge.component';
import { admGuard } from '../../guards/adm.guard';
import { ListComponent } from './routes/list/list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list', pathMatch: 'full' },
  { path: 'badge', component: BadgeComponent, canActivate: [admGuard] },
  { path: 'list', component: ListComponent },
  { path: '**', redirectTo: 'list' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PrintsRoutingModule { }
