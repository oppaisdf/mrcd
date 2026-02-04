import { Routes } from "@angular/router";
import { BaseEntitiesService } from "../../shared/baseEntities/services/base-entities.service";

export const DOCUMENTS_ROUTES: Routes = [
    {
        path: '',
        title: 'Documentos',
        data: {
            vtIndex: 3001
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./list/document-list.page').then(p => p.DocumentListPage)
    }, {
        path: 'new',
        title: 'Nuevo documento',
        data: {
            vtIndex: 3002
        },
        providers: [
            BaseEntitiesService
        ],
        loadComponent: () => import('./create/documents-create.page').then(p => p.DocumentsCreatePage)
    }
];