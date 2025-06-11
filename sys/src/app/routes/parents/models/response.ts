import { BasicPersonResponse } from "../../charges/models/responses/charge";

export interface ParentResponse {
    id: number;
    name: string;
    isParent: boolean;
    gender: boolean;
    phone?: string;
    people?: BasicPersonResponse[];
}