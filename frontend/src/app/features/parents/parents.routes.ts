import { Routes } from "@angular/router";
import { ParentService } from "./services/parent.service";
import { guidParamGuard } from "../../core/guards/guid-param-guard";

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
    }, {
        path: ':id',
        title: 'Detalles del padre/padrino',
        canMatch: [guidParamGuard('id')],
        data: { vtIndex: 9003 },
        providers: [ParentService],
        loadComponent: () => import('./details/parents-details.page').then(p => p.ParentsDetailsPage)
    }
];