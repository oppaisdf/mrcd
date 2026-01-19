import { HttpErrorResponse } from "@angular/common/http";
import { ProblemDetails } from "./api.types";

export function parseAPIError(
    error: unknown
): {
    status?: number;
    message: string;
    details?: ProblemDetails | null;
} {
    if (!(error instanceof HttpErrorResponse))
        return { message: 'Error desconocido' };

    const status = error.status;
    const payload = error.error;

    if (payload && typeof payload === 'object') {
        const pd = payload as ProblemDetails;
        const message = pd.detail || pd.title || `Error: ${status}`;
        return { status, message, details: pd };
    }

    if (typeof payload === 'string')
        return { status, message: payload };

    return { status, message: error.message || `Error: ${status}` };
}
