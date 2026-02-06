import { UsedRoleResponse } from "../../roles/responses/UsedRole.response";

export type UserDTO = {
    id: string;
    username: string;
    isActive: boolean;
    roles: Array<UsedRoleResponse>;
};