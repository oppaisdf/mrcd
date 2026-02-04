import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell.component/shell.component';
import { guestGuard } from './core/guards/guest-guard';
import { authGuard } from './core/guards/auth-guard';
import { roleGuard } from './core/guards/role-guard';

export const routes: Routes = [
    {
        path: 'login',
        canMatch: [guestGuard],
        data: {
            vtIndex: 0
        },
        loadComponent: () => import('./features/login.page/login.page').then(p => p.LoginPage)
    },
    {
        path: '',
        canMatch: [authGuard],
        component: ShellComponent,
        children: [
            {
                path: 'permissions',
                canMatch: [roleGuard],
                data: {
                    roles: ['sys'],
                    vtIndex: 10,
                    vtTheme: 'system'
                },
                loadChildren: () => import('./features/permissions/permissions.routes').then(r => r.PERMISSIONS_ROUTES)
            }, {
                path: 'roles',
                canMatch: [roleGuard],
                data: {
                    roles: ['sys'],
                    vtIndex: 20,
                    vtTheme: 'system'
                },
                loadChildren: () => import('./features/roles/roles.routes').then(r => r.ROLES_ROUTES)
            }, {
                path: 'documents',
                canMatch: [roleGuard],
                data: {
                    roles: ['adm'],
                    vtIndex: 30,
                    vtTheme: 'admin'
                },
                loadChildren: () => import('./features/documents/documnts.routes').then(r => r.DOCUMENTS_ROUTES)
            }, {
                path: 'degrees',
                canMatch: [roleGuard],
                data: {
                    roles: ['adm'],
                    vtIndex: 40,
                    vtTheme: 'admin'
                },
                loadChildren: () => import('./features/degrees/degrees.routes').then(r => r.DEGREES_ROUTES)
            }, {
                path: 'sacraments',
                canMatch: [roleGuard],
                data: {
                    roles: ['adm'],
                    vtIndex: 50,
                    vtTheme: 'admin'
                },
                loadChildren: () => import('./features/sacraments/sacraments.routes').then(r => r.SACRAMENTS_ROUTES)
            }
        ]
    },
    {
        path: 'not-found',
        title: 'Not found',
        loadComponent: () => import('./features/not-found.page/not-found.page').then(p => p.NotFoundPage)
    }, {
        path: 'forbidden',
        title: 'Forbidden',
        loadComponent: () => import('./features/forbidden.page/forbidden.page').then(p => p.ForbiddenPage)
    },
    { path: '**', redirectTo: 'not-found' }
];
