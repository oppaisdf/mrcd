import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AllComponent } from './routes/all/all.component';
import { NewComponent } from './routes/new/new.component';
import { DetailsComponent } from './routes/details/details.component';
import { admGuard } from '../../guards/adm.guard';
import { detailGuard } from './guards/detail.guard';

const routes: Routes = [
  { path: '', redirectTo: 'all', pathMatch: 'full' },
  { path: 'all', component: AllComponent },
  { path: 'new', component: NewComponent, canActivate: [admGuard] },
  { path: ':id', component: DetailsComponent, canActivate: [detailGuard] },
  { path: '**', redirectTo: 'all' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentsRoutingModule { }
