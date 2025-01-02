export interface UserRequest {
    username?: string;
    password?: string;
    isActive?: boolean;
    roles?: string[];
}