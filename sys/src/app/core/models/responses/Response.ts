export interface DefaultResponse {
    id: number;
    name: string;
}

export interface NavItem {
    label: string;
    expanded: boolean;
    route: string;
    show: boolean;
    children?: NavItem[];
}