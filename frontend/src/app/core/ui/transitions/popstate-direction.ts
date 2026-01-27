import { Injectable, NgZone } from '@angular/core';
import { ViewTransitionService } from './view-transitions.service';

@Injectable({ providedIn: 'root' })
export class PopstateDirection {
    constructor(zone: NgZone, vt: ViewTransitionService) {
        zone.runOutsideAngular(() => {
            window.addEventListener('popstate', () => {
                vt.setBack();
            });
        });
    }
}
