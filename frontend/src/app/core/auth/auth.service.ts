import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SessionStore } from '../stores/session.store';
import { ApiService } from '../api/api.service';
import { LoginRequest } from './login.request';

type LoginResponse = {
    token: string;
    expiresAtUtc: string;
    roles?: string[];
};

function decodeJwtRoles(token: string) {
    const parts = token.split('.');
    if (parts.length < 2) return [];
    try {
        const payloadJson = atob(parts[1].replace(/-/g, '+').replace(/_/g, '/'));
        const payload = JSON.parse(payloadJson);
        const r =
            payload['role'] ??
            payload['roles'] ??
            payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        if (Array.isArray(r)) return r;
        if (typeof r === 'string') return [r];
        return [];
    } catch {
        return [];
    }
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private readonly api = inject(ApiService);
    private readonly store = inject(SessionStore);
    private readonly router = inject(Router);

    async loginAsync(
        username: string,
        password: string
    ) {
        const res = await this.api.postAsync<LoginRequest, LoginResponse>('/auth/login', { username, password });

        if (!res.isSuccess || !res.data?.token)
            throw new Error(res.message ?? 'Login failed');

        const token = res.data.token;
        const roles = res.data.roles?.length ? res.data.roles : decodeJwtRoles(token);

        this.store.setSession({
            token,
            expiresAtUtc: res.data.expiresAtUtc,
            roles
        });
    }

    logout(
        returnUrl?: string
    ): void {
        this.store.clear();
        const extras = returnUrl ? { queryParams: { returnUrl } } : undefined;
        this.router.navigate(['/login'], extras);
    }
}
