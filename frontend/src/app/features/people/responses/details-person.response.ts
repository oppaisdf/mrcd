import { AssignedBaseEntityResponse } from "../../../shared/baseEntities/reponses/Assigned-BaseEntity.response";
import { AssignedParentResponse } from "../../parents/responses/assigned-parent.response";
import { PersonResponse } from "./person.response";

export interface DetailsPersonResponse extends PersonResponse {
    parents: Array<AssignedParentResponse>;
    godparents: Array<AssignedParentResponse>;
    charges: Array<AssignedBaseEntityResponse>;
    documents: Array<AssignedBaseEntityResponse>;
    sacraments: Array<AssignedBaseEntityResponse>;
}