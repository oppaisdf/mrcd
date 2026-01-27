import { Injectable, computed, signal } from '@angular/core';

export type Theme = 'dark' | 'light';

@Injectable({ providedIn: 'root' })
export class ThemeService {
    private readonly storageKey = 'theme';
    private readonly _theme = signal<Theme>(this.readInitialTheme());

    readonly theme = computed(() => this._theme());

    constructor() {
        this.apply(this._theme());
    }

    toggle(): void {
        this.set(this._theme() === 'dark' ? 'light' : 'dark');
    }

    set(theme: Theme): void {
        this._theme.set(theme);
        this.apply(theme);
        try {
            sessionStorage.setItem(this.storageKey, theme);
        } catch { }
    }

    private apply(theme: Theme): void {
        document.documentElement.setAttribute('data-theme', theme);
    }

    private readInitialTheme(): Theme {
        try {
            const saved = sessionStorage.getItem(this.storageKey);
            if (saved === 'dark' || saved === 'light') return saved;
        } catch { }

        const prefersDark = window.matchMedia?.('(prefers-color-scheme: dark)')?.matches ?? false;
        return prefersDark ? 'dark' : 'light';
    }
}
