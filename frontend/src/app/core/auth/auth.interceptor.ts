import { HttpInterceptorFn, HttpRequest } from "@angular/common/http";
import { inject } from "@angular/core";
import { SessionStore } from "../stores/session.store";

const SKIP_AUTH_PATHS = [
    "/auth/login",
    "/health"
];

function shouldSkipAuth(
    req: HttpRequest<unknown>
) {
    return SKIP_AUTH_PATHS.some(r => req.url.includes(r));
}

export const apiAuthInterceptor: HttpInterceptorFn = (req, next) => {
    if (shouldSkipAuth(req))
        return next(req);

    const session = inject(SessionStore);
    const token = session.session()?.token;
    if (!token) return next(req);
    const reqAuth = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
    });
    return next(reqAuth);
};