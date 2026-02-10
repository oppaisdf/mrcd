import { Routes } from "@angular/router";
import { ChargesService } from "./services/charges.service";

export const CHARGES_ROUTES: Routes = [
    {
        path: '',
        title: 'Cobros',
        data: { vtIndex: 8001 },
        providers: [ChargesService],
        loadComponent: () => import('./list/charges-list.page').then(p => p.ChargesListPage)
    }, {
        path: 'new',
        title: 'Crear cobro',
        data: { vtIndex: 8002 },
        providers: [ChargesService],
        loadComponent: () => import('./create/charges-create.page').then(p => p.ChargesCreatePage)
    }
];