export interface LogResponse {
    user: string;
    date: Date;
    action: string;
    details?: string;
}

export interface FilterLogResponse {
    users: UserFilterResponse[];
    actions: ActionFilterResponse[];
}

export interface UserFilterResponse {
    id: string;
    name: string;
}

export interface ActionFilterResponse {
    id: number;
    name: string;
}