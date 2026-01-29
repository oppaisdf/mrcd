import { Routes } from "@angular/router";

export const PERMISSIONS_ROUTES: Routes = [
    {
        path: '',
        pathMatch: 'full',
        title: 'List permissions',
        data: {
            vtIndex: 1001,
            vtTheme: 'permissions'
        },
        loadComponent: () => import('./list/permissions-list.page').then(p => p.PermissionsListPage)
    }, {
        path: 'new',
        title: 'Create permission',
        data: {
            vtIndex: 1002,
            vtTheme: 'permissions'
        },
        loadComponent: () => import('./create/permissions-create.page').then(p => p.PermissionsCreatePage)
    }
];