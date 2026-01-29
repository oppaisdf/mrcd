import { Routes } from "@angular/router";

export const ROLES_ROUTES: Routes = [
    {
        path: '',
        title: 'List roles',
        data: {
            vtIndex: 2001,
            vtTheme: 'roles'
        },
        loadComponent: () => import('./list/roles-list.page').then(p => p.RolesListPage)
    }, {
        path: 'new',
        title: 'Create role',
        data: {
            vtIndex: 2002,
            vtTheme: 'roles'
        },
        loadComponent: () => import('./create/roles-create.page').then(p => p.RolesCreatePage)
    }
];