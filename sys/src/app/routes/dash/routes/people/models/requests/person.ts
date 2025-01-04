export interface PersonRequest {
    name?: string;
    gender?: boolean;
    dob?: string;
    day?: boolean;
    parish?: string;
    degreeId?: number;
    address?: string;
    phone?: string;
    isActive?: boolean;
    sacraments?: number[];
    parents?: ParentRequest[];
    godparents?: ParentRequest[];
}

export interface ParentRequest {
    name: string;
    gender: boolean;
    phone?: string;
}