import { Routes } from "@angular/router";
import { BaseEntitiesService } from "../../shared/baseEntities/services/base-entities.service";
import { PersonService } from "./services/person.service";

export const PEOPLE_ROUTES: Routes = [
    {
        path: '',
        title: 'Confirmandos',
        data: { vtIndex: 7002 },
        providers: [PersonService],
        loadComponent: () => import('./list/people-list.page').then(p => p.PeopleListPage)
    }, {
        path: 'new',
        title: 'Inscripción',
        data: {
            vtIndex: 7001
        },
        providers: [
            BaseEntitiesService,
            PersonService
        ],
        loadComponent: () => import('./create/person-create.page').then(p => p.PersonCreatePage)
    }
];