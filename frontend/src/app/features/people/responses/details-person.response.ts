import { AssignedBaseEntityResponse } from "../../../shared/baseEntities/reponses/Assigned-BaseEntity.response";
import { PersonResponse } from "./person.response";

export interface DetailsPersonResponse extends PersonResponse {
    documents: Array<AssignedBaseEntityResponse>;
    sacraments: Array<AssignedBaseEntityResponse>;
}