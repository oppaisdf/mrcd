import { Injectable, inject } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { NavDirection, NavDirectionService } from './nav-direction.service';

@Injectable({ providedIn: 'root' })
export class ViewTransitionService {
    private readonly router = inject(Router);
    private readonly dir = inject(NavDirectionService);

    async navigate(
        commands: any[] | string,
        opts?: { direction?: NavDirection; extras?: NavigationExtras }
    ): Promise<boolean> {
        const direction = opts?.direction ?? 'forward';
        const extras = opts?.extras;

        this.dir.setProgrammatic(direction);

        const nav = async () => {
            if (typeof commands === 'string') {
                return this.router.navigateByUrl(commands, extras);
            }
            return this.router.navigate(commands, extras);
        };

        const doc: any = document;
        const startVT = (doc.startViewTransition as Function | undefined)?.bind(document);

        if (!startVT) {
            const ok = await nav();
            if (ok) {
                extras?.replaceUrl ? this.dir.commitReplace() : this.dir.commitPush();
            }
            queueMicrotask(() => this.dir.reset());
            return ok;
        }

        const vt = startVT(async () => {
            const ok = await nav();
            if (ok) {
                extras?.replaceUrl ? this.dir.commitReplace() : this.dir.commitPush();
            }
            return ok;
        });

        try {
            await vt.finished;
        } catch {
        } finally {
            this.dir.reset();
        }

        return true;
    }
}
