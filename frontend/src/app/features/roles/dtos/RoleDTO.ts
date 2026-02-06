import { PermissionResponse } from "../../permissions/reponses/Permission.response";

export type RoleDTO = {
    roleID: string;
    roleName: string;
    permissions: Array<PermissionResponse>;
};