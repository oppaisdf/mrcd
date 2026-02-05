import { Routes } from "@angular/router";
import { UserService } from "./services/user.service";

export const USERS_ROUTES: Routes = [
    {
        path: '',
        title: 'Usuarios',
        data: {
            vtIndex: 6001
        },
        providers: [
            UserService
        ],
        loadComponent: () => import('./list/users-list.page').then(p => p.UsersListPage)
    }
];