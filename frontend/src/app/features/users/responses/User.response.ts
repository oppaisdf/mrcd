import { UsedRoleResponse } from "../../roles/responses/UsedRole.response";

export type UserResponse = {
    id: string;
    username: string;
    isActive: boolean;
    roles: Array<UsedRoleResponse>;
};