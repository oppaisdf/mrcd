import { Routes } from "@angular/router";
import { AccountingMovementService } from "./services/accounting-movement.service";

export const ACCOUNTING_MOVEMENT_ROUTES: Routes = [
    {
        path: 'new',
        title: 'Agregar movimiento contable',
        data: {vtIndex: 132},
        providers: [AccountingMovementService],
        loadComponent: () => import('./create/accounting-movement-create.page').then(p => p.AccountingMovementCreatePage)
    }, {
        path: 'list',
        title: 'Ver movimientos contables',
        data: {vtIndex: 131},
        providers: [AccountingMovementService],
        loadComponent: () => import('./list/accounting-movement-list.page').then(p => p.AccountingMovementListPage)
    }, {
        path: '**',
        redirectTo: 'list'
    }
];