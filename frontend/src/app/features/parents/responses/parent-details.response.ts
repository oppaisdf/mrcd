import { SimplePersonResponse } from "../../people/responses/simple-person.response";

export type ParentDetailsResponse = {
    name: string;
    isMasculine: boolean;
    phone: string;
    children: Array<SimplePersonResponse>;
    goodchildre: Array<SimplePersonResponse>;
};