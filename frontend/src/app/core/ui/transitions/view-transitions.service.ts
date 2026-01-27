import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

export type NavDirection = 'forward' | 'back' | 'none';

@Injectable({ providedIn: 'root' })
export class ViewTransitionService {
    private _lastIndex = this.readIndex();

    constructor(private readonly router: Router) { }

    async navigate(
        commands: any[] | string,
        opts?: { direction?: NavDirection }
    ): Promise<boolean> {
        const direction = opts?.direction ?? 'none';
        document.documentElement.dataset['navDir'] = direction;

        const nav = async () => {
            if (typeof commands === 'string') {
                return this.router.navigateByUrl(commands);
            }
            return this.router.navigate(commands);
        };

        const startVT = (document as any).startViewTransition as undefined | ((cb: () => Promise<any> | any) => any);

        if (!startVT) {
            const ok = await nav();
            queueMicrotask(() => (document.documentElement.dataset['navDir'] = 'none'));
            return ok;
        }

        const vt = startVT(async () => {
            const ok = await nav();
            this.bumpIndexForPush();
            return ok;
        });

        try {
            await vt.finished;
        } catch {
        } finally {
            document.documentElement.dataset['navDir'] = 'none';
        }

        return true;
    }

    setBack(): void {
        document.documentElement.dataset['navDir'] = 'back';
    }

    private bumpIndexForPush(): void {
        this._lastIndex = this._lastIndex + 1;
        this.writeIndex(this._lastIndex);
    }

    private readIndex(): number {
        const idx = history.state?.__navIndex;
        return typeof idx === 'number' ? idx : 0;
    }

    private writeIndex(idx: number): void {
        const next = { ...(history.state ?? {}), __navIndex: idx };
        history.replaceState(next, '', location.href);
    }
}
