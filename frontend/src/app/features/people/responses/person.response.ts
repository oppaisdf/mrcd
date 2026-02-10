export interface PersonResponse {
    name: string;
    isActive: boolean;
    isMasculine: boolean;
    isSunday: boolean;
    dob: Date;
    degreeId: string;
    parish?: string;
    address?: string;
    phone?: string;
};