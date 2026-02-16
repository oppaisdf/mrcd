import { Routes } from "@angular/router";
import { ParentService } from "./services/parent.service";

export const PARENTS_ROUTES: Routes = [
    {
        path: '',
        title: 'Padres y padrinos',
        data: { vtIndex: 9001 },
        providers: [
            ParentService
        ],
        loadComponent: () => import('./list/list-parents.page').then(p => p.ListParentsPage)
    }, {
        path: 'new',
        title: 'Nuevo padre/padrino',
        data: { vtIndex: 9002 },
        providers: [
            ParentService
        ],
        loadComponent: () => import('./create/create-parents.page').then(p => p.CreateParentsPage)
    }
];