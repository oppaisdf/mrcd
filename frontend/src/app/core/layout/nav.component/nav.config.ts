import { RoleType } from "../../auth/role.type";

export type CategoryId =
    'Confirmandos'
    | 'Impresiones'
    | 'Gestiones'
    | 'Monitoreo'
    | 'System'
    | 'home';

export interface CategoryMenu {
    id: CategoryId,
    icon: string,
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
                route: "/documents",
                roles: ['adm']
            }, {
                routeName: "Fases de actividad",
                route: "/",
                roles: ['usr']
            }, {
                routeName: "Grados académicos",
                route: "/degrees",
                roles: ['adm']
            }, {
                routeName: "Movimientos contables",
                route: "/",
                roles: ['adm']
            }, {
                routeName: "Usuarios",
                route: "/users",
                roles: ['adm']
            }, {
                routeName: "Sacramentos",
                route: "/sacraments",
                roles: ['adm']
            }
        ]
    }, {
        id: "Monitoreo",
        icon: "🖥️",
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
        roles: ['sys'],
        options: [
            {
                routeName: "Roles",
                route: "/roles",
                roles: ['sys']
            }, {
                routeName: "Permissions",
                route: "/permissions",
                roles: ['sys']
            }
        ]
    }
];
