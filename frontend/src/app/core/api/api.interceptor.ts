import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { catchError, throwError } from "rxjs";
import { AuthService } from "../auth/auth.service";
import { Router } from "@angular/router";
import { AlertService } from "../../shared/alerts/services/alert.service";

export const apiErrorInterceptor: HttpInterceptorFn = (req, next) => {
    const auth = inject(AuthService);
    const router = inject(Router);
    const alert = inject(AlertService);

    return next(req).pipe(
        catchError((err: HttpErrorResponse) => {
            switch (err.status) {
                case 403:
                    alert.clear();
                    router.navigateByUrl('/forbidden');
                    return throwError(() => ({ silent: true, original: err }));
                case 401:
                    alert.clear();
                    auth.logout();
                    return throwError(() => ({ silent: true, original: err }));
                default:
                    return throwError(() => err);
            }
        })
    );
};