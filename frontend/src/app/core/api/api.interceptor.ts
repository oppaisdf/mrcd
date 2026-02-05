import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { catchError, throwError } from "rxjs";
import { AuthService } from "../auth/auth.service";
import { Router } from "@angular/router";

export const apiErrorInterceptor: HttpInterceptorFn = (req, next) => {
    const auth = inject(AuthService);
    const router = inject(Router);

    return next(req).pipe(
        catchError((err: HttpErrorResponse) => {
            switch (err.status) {
                case 403:
                    router.navigateByUrl('/forbidden');
                    break;
                case 401:
                    auth.logout();
                    break;
            }
            return throwError(() => err);
        })
    );
};