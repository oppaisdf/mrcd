import { computed, Injectable, signal } from '@angular/core';
import { AlertItem } from './alert.item';

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
    this._stack.set(error);
  }
}
