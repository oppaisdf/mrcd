import { Routes } from "@angular/router";
import { PersonService } from "../people/services/person.service";

export const PRINTS_ROUTES: Routes = [
    {
        path: 'badges',
        title: 'Gafetes',
        data: { vtIndex: 1011 },
        providers: [PersonService],
        loadComponent: () => import('./badges/badges.page').then(p => p.BadgesPage)
    }
];