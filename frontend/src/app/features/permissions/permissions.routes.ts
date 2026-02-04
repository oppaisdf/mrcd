import { Routes } from "@angular/router";
import { BaseEntitiesService } from "../../shared/baseEntities/services/base-entities.service";

export const PERMISSIONS_ROUTES: Routes = [
    {
        path: '',
        pathMatch: 'full',
        title: 'List permissions',
        data: {
            vtIndex: 1001
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./list/permissions-list.page').then(p => p.PermissionsListPage)
    }, {
        path: 'new',
        title: 'Create permission',
        data: {
            vtIndex: 1002
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./create/permissions-create.page').then(p => p.PermissionsCreatePage)
    }
];