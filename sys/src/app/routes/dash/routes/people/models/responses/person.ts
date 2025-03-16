import { ChargeResponse } from "../../../charges/models/responses/charge";
import { ParentResponse } from "../../../parents/models/response";

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
    sacraments: DefaultEntityStatusResponse[];
    degrees?: DefaultEntityResponse[];
    charges?: ChargeResponse[];
    documents?: DefaultEntityStatusResponse[];
}

export interface DefaultEntityStatusResponse {
    id: number;
    name: string;
    isActive: boolean;
}

export interface PersonFilterResponse {
    degrees: DefaultEntityResponse[];
    sacraments: DefaultEntityResponse[];
    price?: number;
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