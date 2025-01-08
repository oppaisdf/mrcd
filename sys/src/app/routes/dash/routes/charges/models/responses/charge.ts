export interface ChargeResponse {
    id: number;
    name: string;
    total: number;
    isActive: boolean;
    people?: BasicPersonResponse[];
}

export interface BasicPersonResponse {
    id: number;
    name: string;
    isActive: boolean;
    gender: boolean;
    day: boolean;
    hasParent?: boolean;
}