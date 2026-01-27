import { Injectable, NgZone } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

export type NavDirection = 'forward' | 'back' | 'none';

@Injectable({ providedIn: 'root' })
export class NavDirectionService {
    private readonly key = '__navIndex';
    private _lastIndex = this.readIndex();

    private _dir: NavDirection = 'none';

    constructor(router: Router, zone: NgZone) {
        // Detecta back/forward real (popstate) vía NavigationStart
        router.events
            .pipe(filter((e): e is NavigationStart => e instanceof NavigationStart))
            .subscribe((e) => {
                // Si el trigger viene del navegador (atrás/adelante)
                if (e.navigationTrigger === 'popstate') {
                    const idx = this.readIndex();
                    const dir: NavDirection =
                        idx < this._lastIndex ? 'back' :
                            idx > this._lastIndex ? 'forward' :
                                'none';

                    this._dir = dir;
                    this._lastIndex = idx;
                    this.apply(dir);
                }
            });

        zone.runOutsideAngular(() => {
            this.apply('none');
        });
    }

    setProgrammatic(direction: NavDirection): void {
        this._dir = direction;
        this.apply(direction);
    }

    commitPush(): void {
        const next = this._lastIndex + 1;
        this._lastIndex = next;
        this.writeIndex(next);
    }

    commitReplace(): void {
        this.writeIndex(this._lastIndex);
    }

    reset(): void {
        this._dir = 'none';
        this.apply('none');
    }

    private apply(dir: NavDirection): void {
        document.documentElement.dataset['navDir'] = dir;
    }

    private readIndex(): number {
        const idx = history.state?.[this.key];
        return typeof idx === 'number' ? idx : 0;
    }

    private writeIndex(idx: number): void {
        const next = { ...(history.state ?? {}), [this.key]: idx };
        history.replaceState(next, '', location.href);
    }
}
