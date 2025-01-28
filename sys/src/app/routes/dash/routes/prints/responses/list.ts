export interface ListGeneralResponse {
    name: string;
    gender: boolean;
    day: boolean;
    dob: Date;
    phone?: string;
    parents?: GeneralParentListResponse[];
}

export interface GeneralParentListResponse {
    name: string;
    phone?: string;
}