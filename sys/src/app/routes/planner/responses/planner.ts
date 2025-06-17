export interface DayResponse {
    day: number;
    activities?: SimpleActivityResponse[];
}

export interface SimpleActivityResponse {
    id: number;
    name: string;
}