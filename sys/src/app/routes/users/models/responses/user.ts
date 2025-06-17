export interface UserResponse {
    id: string;
    username: string;
    isActive: boolean;
    roles: string[];
}

export interface SimpleUserResponse {
    id: string;
    name: string;
}