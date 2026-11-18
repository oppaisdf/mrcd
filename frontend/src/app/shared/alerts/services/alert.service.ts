import { computed, Injectable, signal } from '@angular/core';
import { AlertItem, ConfirmationAction } from './alert.item';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private readonly _stack = signal<AlertItem | null>(null);
  readonly alert = computed(() => this._stack());
  readonly isOpen = computed(() => this.alert() !== null);
  readonly loading = signal<boolean>(false);

  clear() {
    this._stack.set(null);
    this.loading.set(false);
  }

  public startLoading() {
    const loading: AlertItem = {
      type: 'loading',
      title: 'Cargando...',
      message: ''
    };
    this._stack.set(loading);
    this.loading.set(true);
  }

  public success(
    message: string
  ) {
    const success: AlertItem = {
      type: 'success',
      title: 'Éxito',
      message: message
    };
    this.loading.set(false);
    this._stack.set(success);
  }

  public error(
    message?: string
  ) {
    if (!message) return;
    const error: AlertItem = {
      type: 'error',
      title: 'Error',
      message: message
    };
    this.loading.set(false);
    this._stack.set(error);
  }
  /** Show a question; only explicit acceptance executes the action. */
  public confirm(
    message: string,
    onConfirm: ConfirmationAction,
    title = '¿Continuamos?'
  ): void {
    this.loading.set(false);
    this._stack.set({ type: 'confirmation', title, message, onConfirm });
  }

  public async acceptConfirmation(): Promise<void> {
    const current = this._stack();
    if (current?.type !== 'confirmation') return;

    // Consume before invoking: repeated clicks cannot execute it twice, and the
    // callback can show loading or another alert without being cleared afterward.
    this.clear();
    try {
      await current.onConfirm();
    } catch {
      this.error('No se pudo completar la acción. Inténtalo de nuevo.');
    }
  }
}
