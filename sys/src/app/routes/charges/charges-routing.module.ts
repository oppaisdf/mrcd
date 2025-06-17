import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { DetailComponent } from './routes/detail/detail.component';
import { admGuard } from '../../core/guards/adm.guard';
import { detailsGuard } from '../../core/guards/details-guard';

const routes: Routes = [
  { path: '', redirectTo: 'all', pathMatch: 'full' },
  { path: 'all', component: AllComponent },
  { path: 'new', component: NewComponent, canActivate: [admGuard] },
  { path: ':id', component: DetailComponent, canActivate: [detailsGuard] },
  { path: '**', redirectTo: 'all' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChargesRoutingModule { }
