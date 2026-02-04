import { Routes } from "@angular/router";
import { BaseEntitiesService } from "../../shared/baseEntities/services/base-entities.service";

export const DEGREES_ROUTES: Routes = [
    {
        path: '',
        title: 'Grados académicos',
        data: {
            vtIndex: 4001
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./list/degrees-list.page').then(p => p.DegreesListPage)
    }, {
        path: 'new',
        title: 'Nuevo grado académico',
        data: {
            vtIndex: 4002
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./create/degrees-create.page').then(p => p.DegreesCreatePage)
    }
];