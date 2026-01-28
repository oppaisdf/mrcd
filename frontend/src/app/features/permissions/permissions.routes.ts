import { Routes } from "@angular/router";

export const PERMISSIONS_ROUTES: Routes = [
    {
        path: '',
        pathMatch: 'full',
        title: 'List permissions',
        loadComponent: () => import('./list/permissions-list.page').then(p => p.PermissionsListPage)
    }, {
        path: 'new',
        title: 'Create permission',
        loadComponent: () => import('./create/permissions-create.page').then(p => p.PermissionsCreatePage)
    }
];