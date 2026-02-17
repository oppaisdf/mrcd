import { Routes } from "@angular/router";

export const ATTENDANCES_ROUTES: Routes = [
    {
        path: '',
        title: 'Asistencia',
        data: { vtIndex: 1001 },
        loadComponent: () => import('./add/add-attendances.page').then(p => p.AddAttendancesPage)
    }
];