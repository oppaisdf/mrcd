export type CreateUserRequest = {
    username: string;
    password: string;
    roles: Array<string>;
};