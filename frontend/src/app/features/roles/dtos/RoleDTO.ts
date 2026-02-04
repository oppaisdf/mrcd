import { PermissionDTO } from "./PermissionDTO";

export type RoleDTO = {
    roleID: string;
    roleName: string;
    permissions: PermissionDTO[];
};