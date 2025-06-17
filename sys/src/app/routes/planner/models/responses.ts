import { DefaultResponse } from "../../../core/models/responses/Response";
import { SimpleUserResponse } from "../../users/models/responses/user";

export interface DayResponse {
    day: number;
    activities?: SimpleActivityResponse[];
}

export interface SimpleActivityResponse {
    id: number;
    name: string;
}
export interface MonthResponse {
    name: string;
    value: number;
}

export interface ActivityStageResponse {
    stageId: number;
    name: string;
    mainUser: boolean;
    userId?: string;
    notes?: string;
}

export interface ActivityResponse {
    name: string;
    date: Date;
    activities: ActivityStageResponse[];
}

export interface PlannerResponse {
    activity: ActivityResponse;
    stages: DefaultResponse[];
    users: SimpleUserResponse[];
}