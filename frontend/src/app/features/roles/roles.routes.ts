import { Routes } from "@angular/router";
import { BaseEntitiesService } from "../../shared/baseEntities/services/base-entities.service";

export const ROLES_ROUTES: Routes = [
    {
        path: '',
        title: 'List roles',
        data: {
            vtIndex: 2001
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./list/roles-list.page').then(p => p.RolesListPage)
    }, {
        path: 'new',
        title: 'Create role',
        data: {
            vtIndex: 2002
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./create/roles-create.page').then(p => p.RolesCreatePage)
    }
];