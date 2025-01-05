import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashComponent } from './routes/base/dash.component';
import { sysGuard } from './guards/sys.guard';
import { admGuard } from './guards/adm.guard';

const routes: Routes = [
  {
    path: '', component: DashComponent, children: [
      { path: 'role', loadChildren: () => import('./routes/roles/roles.module').then(m => m.RolesModule), canActivate: [sysGuard] },
      { path: 'action', loadChildren: () => import('./routes/actions/actions.module').then(m => m.ActionsModule), canActivate: [admGuard] },
      { path: 'degree', loadChildren: () => import('./routes/degrees/degrees.module').then(m => m.DegreesModule), canActivate: [admGuard] },
      { path: 'sacrament', loadChildren: () => import('./routes/sacraments/sacraments.module').then(m => m.SacramentsModule), canActivate: [admGuard] },
      { path: 'logs', loadChildren: () => import('./routes/logs/logs.module').then(m => m.LogsModule), canActivate: [admGuard] },
      { path: 'user', loadChildren: () => import('./routes/users/users.module').then(m => m.UsersModule) },
      { path: 'person', loadChildren: () => import('./routes/people/people.module').then(m => m.PeopleModule) },
      { path: 'print', loadChildren: () => import('./routes/prints/prints.module').then(m => m.PrintsModule) }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashRoutingModule { }
