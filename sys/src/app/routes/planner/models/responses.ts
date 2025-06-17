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