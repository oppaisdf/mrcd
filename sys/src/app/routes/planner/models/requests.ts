export interface ActivityRequest {
    name: string;
    date: Date;
}

export interface ActivityStageRequest {
    activityId: number;
    stageId: number;
    mainUser: boolean;
    userId?: string;
    notes?: string;
}