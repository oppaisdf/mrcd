import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { dashGuard } from './core/guards/dash.guard';
import { LoginComponent } from './routes/login/login.component';
import { loginGuard } from './core/guards/login.guard';

const routes: Routes = [
  { path: '', loadChildren: () => import('./routes/dash/dash.module').then(m => m.DashModule), canActivate: [dashGuard] },
  { path: 'login', component: LoginComponent, canActivate: [loginGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
