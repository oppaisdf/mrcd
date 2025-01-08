import { BasicPersonResponse } from "../../charges/models/responses/charge";

export interface ParentResponse {
    id: number;
    name: string;
    gender: boolean;
    phone?: string;
    people?: BasicPersonResponse[];
}