import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell.component/shell.component';
import { guestGuard } from './core/guards/guest-guard';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
    {
        path: 'login',
        canMatch: [guestGuard],
        loadComponent: () => import('./features/login.page/login.page').then(p => p.LoginPage)
    },
    {
        path: '',
        canMatch: [authGuard],
        component: ShellComponent
    },
    { path: '**', redirectTo: '' }
];
