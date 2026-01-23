import { RoleType } from "../../auth/role.type";

export type CategoryId =
    'Confirmandos'
    | 'Impresiones'
    | 'Gestiones'
    | 'Monitoreo'
    | 'System';

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
        id: 'Confirmandos',
        icon: '🏠',
        route: '/',
        roles: ['usr'],
        options: [
            {
                routeName: 'Inscribir',
                route: '/',
                roles: ['usr']
            }, {
                routeName: 'Actualizar',
                route: '/',
                roles: ['usr']
            }, {
                routeName: 'Asistencia',
                route: '/',
                roles: ['usr']
            }, {
                routeName: 'Padres/padrinos',
                route: '/',
                roles: ['usr']
            }
        ]
    }, {
        id: 'Impresiones',
        icon: '🖨️',
        route: '/',
        roles: ['usr'],
        options: [
            {
                routeName: "Agenda",
                route: "/",
                roles: ['usr']
            }, {
                routeName: "Asistencia",
                route: "/",
                roles: ['usr']
            }, {
                routeName: "Diplomas",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Gafetes",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Listado general",
                route: "/",
                roles: ['usr']
            }
        ]
    }, {
        id: "Gestiones",
        icon: "🛠",
        route: "/",
        roles: ['usr'],
        options: [
            {
                routeName: "Agenda",
                route: "/",
                roles: ['usr']
            }, {
                routeName: "Cobros",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Documentos",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Fases de actividad",
                route: "/",
                roles: ['usr']
            }, {
                routeName: "Grados académicos",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Movimientos contables",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Usuarios",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Sacramentos",
                route: "/",
                roles: ['adm']
            }
        ]
    }, {
        id: "Monitoreo",
        icon: "🖥️",
        route: "/",
        roles: ['adm'],
        options: [
            {
                routeName: "Logs",
                route: "/",
                roles: ['adm']
            }
        ]
    }, {
        id: "System",
        icon: "💻",
        route: "/",
        roles: ['sys'],
        options: [
            {
                routeName: "Roles",
                route: "/",
                roles: ['sys']
            }, {
                routeName: "Permissions",
                route: "/",
                roles: ['sys']
            }
        ]
    }
];
