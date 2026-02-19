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
                route: '/people/new',
                roles: ['usr']
            }, {
                routeName: 'Actualizar',
                route: '/people',
                roles: ['usr']
            }, {
                routeName: 'Asistencia',
                route: '/attendances',
                roles: ['usr']
            }, {
                routeName: 'Padres/padrinos',
                route: '/parents',
                roles: ['usr']
            }
        ]
    }, {
        id: 'Impresiones',
        icon: '🖨️',
        roles: ['adm'],
        options: [
            {
                routeName: "Agenda",
                route: "/",
                roles: ['ukn']
            }, {
                routeName: "Asistencia",
                route: "/",
                roles: ['ukn']
            }, {
                routeName: "Diplomas",
                route: "/",
                roles: ['ukn']
            }, {
                routeName: "Gafetes",
                route: "/print/badges",
                roles: ['adm']
            }, {
                routeName: "Listado general",
                route: "/",
                roles: ['ukn']
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
                roles: ['ukn']
            }, {
                routeName: "Cobros",
                route: "/charges",
                roles: ['adm']
            }, {
                routeName: "Documentos",
                route: "/documents",
                roles: ['adm']
            }, {
                routeName: "Fases de actividad",
                route: "/",
                roles: ['ukn']
            }, {
                routeName: "Grados académicos",
                route: "/degrees",
                roles: ['adm']
            }, {
                routeName: "Movimientos contables",
                route: "/",
                roles: ['ukn']
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
        roles: ['ukn'],
        options: [
            {
                routeName: "Logs",
                route: "/",
                roles: ['ukn']
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
