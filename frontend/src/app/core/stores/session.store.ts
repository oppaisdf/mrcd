import { Injectable, computed, signal } from '@angular/core';

export type Session = {
    token: string;
    expiresAtUtc: string; // ISO string
    roles: string[];
};

const KEY = 'app.session';

function parseJson<T>(value: string | null): T | null {
    if (!value) return null;
    try { return JSON.parse(value) as T; } catch { return null; }
}

@Injectable({ providedIn: 'root' })
export class SessionStore {
    private readonly _session = signal<Session | null>(null);

    readonly session = this._session.asReadonly();
    readonly isAuthenticated = computed(() => !!this._session()?.token);
    readonly isExpired = computed(() => {
        const s = this._session();
        if (!s) return true;
        const exp = Date.parse(s.expiresAtUtc);
        return Number.isNaN(exp) ? true : Date.now() >= exp;
    });

    readonly roles = computed(() => this._session()?.roles ?? []);
    hasRole(role: string) { return this.roles().includes(role); }

    loadFromStorage(): void {
        const saved = parseJson<Session>(sessionStorage.getItem(KEY));
        this._session.set(saved);
    }

    setSession(session: Session): void {
        this._session.set(session);
        sessionStorage.setItem(KEY, JSON.stringify(session));
    }

    clear(): void {
        this._session.set(null);
        sessionStorage.removeItem(KEY);
    }
}