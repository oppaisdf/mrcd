import { Routes } from "@angular/router";
import { LogService } from "./services/log.service";

export const LOG_ROUTES: Routes = [
    {
        path: '',
        title: 'Logs',
        data: { vtIndex: 121 },
        providers: [LogService],
        loadComponent: () => import('./list/list-log.page').then(p => p.ListLogPage)
    }
];