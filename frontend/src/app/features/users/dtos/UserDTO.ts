import { UserRoleDTO } from "../../roles/dtos/UserRoleDTO";

export type UserDTO = {
    id: string;
    username: string;
    isActive: boolean;
    roles: Array<UserRoleDTO>;
};