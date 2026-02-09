import { CreateSimpleParentRequest } from "../../parents/requests/create-simple-parent.request";

export type CreatePersonRequest = {
    name: string;
    isMasculine: boolean;
    isSunday: boolean;
    dob: Date;
    address: string;
    phone: string;
    degreeId: string;
    parents: Array<CreateSimpleParentRequest>;
};