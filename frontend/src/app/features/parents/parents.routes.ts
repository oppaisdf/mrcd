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
    }
];