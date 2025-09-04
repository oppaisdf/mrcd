export interface ListGeneralResponse {
    id: number;
    name: string;
    gender: boolean;
    day: boolean;
    dob: Date;
    phone?: string;
    parish?: string;
    address?: string;
    parents?: GeneralParentListResponse[];
    godparents?: GeneralParentListResponse[];
}

export interface GeneralParentListResponse {
    name: string;
    phone?: string;
}