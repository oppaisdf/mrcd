import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { DetailComponent } from './routes/detail/detail.component';
import { MeComponent } from './routes/me/me.component';
import { admGuard } from '../../core/guards/adm.guard';

const routes: Routes = [
  { path: '', redirectTo: 'me', pathMatch: 'full' },
  { path: 'me', component: MeComponent },
  { path: 'all', component: AllComponent, canActivate: [admGuard] },
  { path: 'new', component: NewComponent, canActivate: [admGuard] },
  { path: ':id', component: DetailComponent, canActivate: [admGuard] },
  { path: '**', redirectTo: 'me' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UsersRoutingModule { }
