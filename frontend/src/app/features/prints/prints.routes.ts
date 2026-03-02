import { Routes } from "@angular/router";
import { PersonService } from "../people/services/person.service";

export const PRINTS_ROUTES: Routes = [
    {
        path: 'badges',
        title: 'Gafetes',
        data: { vtIndex: 1011 },
        providers: [PersonService],
        loadComponent: () => import('./badges/badges.page').then(p => p.BadgesPage)
    }, {
        path: 'list',
        title: 'Listado general',
        data: { vtIndex: 1012 },
        providers: [PersonService],
        loadComponent: () => import('./general-list/general-list.page').then(p => p.GeneralListPage)
    }
];