import { DOCUMENT, Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';

type VTDocument = Document & { startViewTransition?: (cb: () => void | Promise<void>) => any };

@Injectable({ providedIn: 'root' })
export class ViewTransitionService {
    private router = inject(Router);
    private doc = inject(DOCUMENT) as VTDocument;

    navigate(event: Event) {
        // Para <a routerLink> para capturar click y meter transición.
        // Se puede solo usar router.navigateByUrl desde código
        event.preventDefault();
        const target = event.currentTarget as HTMLAnchorElement;
        const href = target.getAttribute('href') ?? '/';
        this.go(href);
    }

    async go(url: string) {
        const startVT = this.doc.startViewTransition;
        if (!startVT) {
            await this.router.navigateByUrl(url);
            return;
        }

        await startVT(async () => {
            await this.router.navigateByUrl(url);
        }).finished?.catch(() => void 0);
    }
}
