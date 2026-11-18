import { afterRenderEffect, Component, computed, ElementRef, HostListener, inject, OnDestroy, viewChild } from '@angular/core';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-alert',
  imports: [],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss',
})
export class AlertComponent implements OnDestroy {
  private readonly service = inject(AlertService);
  private readonly panel = viewChild<ElementRef<HTMLElement>>('panel');
  private readonly previousFocus = document.activeElement;
  readonly alert = this.service.alert;
  readonly isBlocking = computed(() => this.alert()?.type === 'loading');

  constructor() {
    afterRenderEffect(() => {
      this.alert();
      const panel = this.panel()?.nativeElement;
      // Cancellation gets initial focus; Enter must never accept by accident.
      (panel?.querySelector<HTMLElement>('button') ?? panel)?.focus();
    });
  }

  close(): void {
    if (!this.isBlocking()) this.service.clear();
  }

  accept(): Promise<void> {
    return this.service.acceptConfirmation();
  }

  @HostListener('document:keydown', ['$event'])
  onKeydown(event: KeyboardEvent): void {
    if (!this.service.isOpen()) return;

    switch (event.key) {
      case 'Escape':
        if (!this.isBlocking()) {
          this.close();
        }
        break;

      case 'Tab':
        this.handleTab(event);
        break;
    }
  }

  private handleTab(event: KeyboardEvent): void {
    const panel = this.panel()?.nativeElement;

    if (!panel) return;

    const buttons = panel.querySelectorAll<HTMLButtonElement>(
      'button:not(:disabled)'
    );

    if (!buttons.length) {
      event.preventDefault();
      panel.focus();
      return;
    }

    const first = buttons[0];
    const last = buttons[buttons.length - 1];

    if (
      event.shiftKey &&
      (document.activeElement === first ||
        document.activeElement === panel)
    ) {
      event.preventDefault();
      last.focus();
    } else if (
      !event.shiftKey &&
      (document.activeElement === last ||
        document.activeElement === panel)
    ) {
      event.preventDefault();
      first.focus();
    }
  }

  ngOnDestroy(): void {
    if (this.previousFocus instanceof HTMLElement && this.previousFocus.isConnected) {
      this.previousFocus.focus();
    }
  }
}
