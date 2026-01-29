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

        try {
            if (typeof commands === 'string')
                return await this.router.navigateByUrl(commands, extras);
            return await this.router.navigate(commands, extras);
        } finally {
            queueMicrotask(() => this.dir.reset());
        }
    }
}
