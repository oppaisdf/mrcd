export interface Result<T> {
    success: boolean;
    message?: string;
    data?: T;
}

export interface ProblemDetails {
    type: string;
    title: string;
    status: number;
    detail: string;
    instance?: string;
    additionalProp1?: string;
    additionalProp2?: string;
    additionalProp3?: string;
}

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    size: number;
    totalPages: number;
    hasPrevious: boolean;
    hasNext: boolean;
}
