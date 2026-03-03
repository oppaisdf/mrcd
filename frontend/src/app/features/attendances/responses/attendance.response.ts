export type AttendanceResponse = {
    personName: string;
    dates: Array<AttendanceDayResponse>;
};

export type AttendanceDayResponse = {
    date: Date;
    type: number;
};