export interface PersonResponse {
    id: number;
    name: string;
    gender: boolean;
    dob: Date;
    day: boolean;
    parish?: string;
    phone: string;
    degreeId: number;
    address?: string;
    isActive: boolean;
    parents?: ParentResponse[];
    godparents?: ParentResponse[];
    sacraments: SacramentResponse[];
    degrees?: DefaultEntityResponse[];
}

export interface ParentResponse {
    id: number;
    name: string;
    gender: boolean;
    phone?: string;
}

export interface SacramentResponse {
    id: number;
    name: string;
    isActive: boolean;
}

export interface PersonFilterResponse {
    degrees: DefaultEntityResponse[];
    sacraments: DefaultEntityResponse[];
}

export interface DefaultEntityResponse {
    id: number;
    name: string;
}

export interface PersonFilterRequest {
    page: number;
    gender?: boolean;
    day?: boolean;
    isActive?: boolean;
    degreeId?: boolean;
    name?: string;
}