import { Routes } from "@angular/router";
import { AttendanceService } from "./services/attendance.service";
import { PersonService } from "../people/services/person.service";

export const ATTENDANCES_ROUTES: Routes = [
    {
        path: '',
        title: 'Asistencia',
        data: { vtIndex: 1001 },
        providers: [AttendanceService],
        loadComponent: () => import('./add/add-attendances.page').then(p => p.AddAttendancesPage)
    }, {
        path: 'manual',
        title: 'Asistencia manual',
        data: { vtIndex: 1002 },
        providers: [
            PersonService,
            AttendanceService
        ],
        loadComponent: () => import('./manual/attendance-manual.page').then(p => p.AttendanceManualPage)
    }
];