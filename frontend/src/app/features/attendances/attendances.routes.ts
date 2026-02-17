import { Routes } from "@angular/router";
import { AttendanceService } from "./services/attendance.service";

export const ATTENDANCES_ROUTES: Routes = [
    {
        path: '',
        title: 'Asistencia',
        data: { vtIndex: 1001 },
        providers: [AttendanceService],
        loadComponent: () => import('./add/add-attendances.page').then(p => p.AddAttendancesPage)
    }
];