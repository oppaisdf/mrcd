import { PermissionResponse } from "../../permissions/reponses/Permission.response";

export type RoleResponse = {
    roleID: string;
    roleName: string;
    permissions: Array<PermissionResponse>;
};