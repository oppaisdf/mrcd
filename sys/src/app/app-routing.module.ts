import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { dashGuard } from './core/guards/dash.guard';
import { LoginComponent } from './routes/login/login.component';
import { loginGuard } from './core/guards/login.guard';
import { DashComponent } from './routes/base/dash.component';
import { sysGuard } from './core/guards/sys.guard';
import { admGuard } from './core/guards/adm.guard';

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
      { path: 'parent', loadChildren: () => import('./routes/parents/parents.module').then(m => m.ParentsModule) },
      { path: 'print', loadChildren: () => import('./routes/prints/prints.module').then(m => m.PrintsModule) },
      { path: 'charge', loadChildren: () => import('./routes/charges/charges.module').then(m => m.ChargesModule) },
      { path: 'attendance', loadChildren: () => import('./routes/attendances/attendances.module').then(m => m.AttendancesModule) },
      { path: 'alerts', loadChildren: () => import('./routes/alerts/alerts.module').then(m => m.AlertsModule) },
      { path: 'docs', loadChildren: () => import('./routes/documents/documents.module').then(m => m.DocumentsModule) }
    ], canActivate: [dashGuard]
  },
  { path: 'login', component: LoginComponent, canActivate: [loginGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
