export interface ChargeResponse {
    id: number;
    name: string;
    total: number;
    isActive: boolean;
    people?: PersonChargeResponse[];
}

export interface PersonChargeResponse {
    id: number;
    name: string;
    isActive: boolean;
    gender: boolean;
    day: boolean;
}