import { RoleType } from "../../auth/role.type";

export type CategoryId = 'main' | 'home';

export interface CategoryMenu {
    id: CategoryId,
    icon: string,
    route: string,
    roles: RoleType[],
    options: OptionMenu[]
}

export interface OptionMenu {
    routeName: string,
    route: string,
    roles: RoleType[]
}

export const NAV_CATEGORIES: CategoryMenu[] = [
    {
        id: 'main',
        icon: '🏠',
        route: '/',
        roles: ['sys'],
        options: [
            {
                routeName: 'Home',
                route: '/',
                roles: ['sys']
            }
        ]
    }, {
        id: 'home',
        icon: '🛠',
        route: '/',
        roles: ['sys'],
        options: []
    }
];
