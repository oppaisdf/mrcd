import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashComponent } from './routes/base/dash.component';
import { sysGuard } from './guards/sys.guard';

const routes: Routes = [
  {
    path: '', component: DashComponent, children: [
      { path: 'role', loadChildren: () => import('./routes/roles/roles.module').then(m => m.RolesModule), canActivate: [sysGuard] }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashRoutingModule { }
