import { Routes } from "@angular/router";

export const ROLES_ROUTES: Routes = [
    {
        path: '',
        title: 'List roles',
        loadComponent: () => import('./list/roles-list.page').then(p => p.RolesListPage)
    }, {
        path: 'new',
        title: 'Create role',
        loadComponent: () => import('./create/roles-create.page').then(p => p.RolesCreatePage)
    }
];