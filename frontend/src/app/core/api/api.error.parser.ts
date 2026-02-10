import { HttpErrorResponse } from "@angular/common/http";
import { ProblemDetails } from "./api.types";

export function parseAPIError(
    error: unknown
): string {
    const wrapper = error as any;
    if (wrapper?.silent) return '';
    if (wrapper?.original) error = wrapper.original;
    if (!(error instanceof HttpErrorResponse))
        return 'Error desconocido';

    const status = error.status;
    const payload = error.error;

    if (payload && typeof payload === 'object') {
        const pd = payload as ProblemDetails;
        const message = pd.detail || pd.title || `Error: ${status}`;
        return message;
    }

    if (typeof payload === 'string')
        return payload;

    return error.message || `Error: ${status}`;
}