import { Routes } from "@angular/router";
import { PersonService } from "../people/services/person.service";
import { AttendanceService } from "../attendances/services/attendance.service";

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
    }, {
        path: 'attendance',
        title: 'Asistencia',
        data: { vtIndex: 1013 },
        providers: [AttendanceService],
        loadComponent: () => import('./attendance/print-attendance.page').then(p => p.PrintAttendancePage)
    }
];