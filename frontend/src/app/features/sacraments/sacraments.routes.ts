import { Routes } from "@angular/router";
import { BaseEntitiesService } from "../../shared/baseEntities/services/base-entities.service";

export const SACRAMENTS_ROUTES: Routes = [
    {
        path: '',
        title: 'Sacramentos',
        data: {
            vtIndex: 5001
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./list/sacraments-list.page').then(p => p.SacramentsListPage)
    }, {
        path: 'new',
        title: 'Nuevo sacramento',
        data: {
            vtIndex: 5002
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./create/sacraments-create.page').then(p => p.SacramentsCreatePage)
    }
];