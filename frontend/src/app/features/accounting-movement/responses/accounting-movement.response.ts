export type AccountingMovementResponse = {
    month: number;
    movements: Array<SimpleAccountingMovementResponse>
};

export type SimpleAccountingMovementResponse = {
    id: string;
    description: string;
    amount: number;
    day: number;
};