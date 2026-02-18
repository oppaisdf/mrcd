import { SimpleParentResposne } from "../../parents/responses/simple-parent.response";

export type GeneralListResponse = {
    personId: string;
    personName: string;
    isMasculine: boolean;
    isSunday: boolean;
    dob: Date;
    registrationDate: Date;
    parish?: string;
    phone?: string;
    parents: Array<SimpleParentResposne>;
    godparents: Array<SimpleParentResposne>;
};