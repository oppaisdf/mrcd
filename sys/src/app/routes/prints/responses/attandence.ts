export interface AttendanceResponse {
    name: string;
    day: boolean;
    gender: boolean;
    dates: DateAttendanceResponse[];
}

export interface DateAttendanceResponse {
    day: number;
    month: number;
    hasAttendance?: boolean;
}