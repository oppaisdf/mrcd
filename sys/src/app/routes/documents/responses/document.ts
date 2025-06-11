import { BasicPersonResponse } from "../../charges/models/responses/charge";

export interface DocumentResponse {
    name: string;
    people: BasicPersonResponse[];
}