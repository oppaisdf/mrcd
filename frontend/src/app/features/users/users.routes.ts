import { Routes } from "@angular/router";
import { UserService } from "./services/user.service";
import { UserRoleService } from "./services/user-role.service";

export const USERS_ROUTES: Routes = [
    {
        path: '',
        title: 'Usuarios',
        data: {
            vtIndex: 6001
        },
        providers: [
            UserService,
            UserRoleService
        ],
        loadComponent: () => import('./list/users-list.page').then(p => p.UsersListPage)
    }, {
        path: 'new',
        title: 'Crear usuario',
        data: {
            vtIndex: 6002
        },
        providers: [
            UserService
        ],
        loadComponent: () => import('./create/users-create.page').then(p => p.UsersCreatePage)
    }
];