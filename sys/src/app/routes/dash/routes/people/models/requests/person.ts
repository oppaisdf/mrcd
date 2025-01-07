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
}

export interface ParentRequest {
    name: string;
    gender: boolean;
    isParent: boolean;
    phone?: string;
}