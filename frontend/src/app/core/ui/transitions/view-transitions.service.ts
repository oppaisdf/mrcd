import { DOCUMENT, Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';

type VTDocument = Document & { startViewTransition?: (cb: () => void | Promise<void>) => any };

@Injectable({ providedIn: 'root' })
export class ViewTransitionService {
    private readonly router = inject(Router);
    private readonly doc = inject(DOCUMENT) as VTDocument;
    private inFlight: Promise<void> | null = null;

    async go(
        url: string
    ) {
        // Si hay una transición corriendo se ignoran nuevos intentos
        if (this.inFlight) return;

        const doc = this.doc;

        const run = async () => {
            if (!doc.startViewTransition) {
                await this.router.navigateByUrl(url);
                return;
            }

            await doc.startViewTransition(async () => {
                await this.router.navigateByUrl(url);
            }).finished?.catch(() => void 0);
        };

        this.inFlight = run().finally(() => (this.inFlight = null));
        await this.inFlight;
    }
}
