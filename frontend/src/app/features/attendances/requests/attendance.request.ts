export type AttendanceRequest = {
    personId: string;
    isAttendance: boolean;
    date?: Date;
};