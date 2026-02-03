import { computed, Injectable, signal } from '@angular/core';
import { AlertItem } from './alert.item';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private readonly _stack = signal<AlertItem | undefined>(undefined);
  readonly alert = computed(() => this._stack());
  readonly isOpen = computed(() => this.alert() !== undefined);

  clear() {
    this._stack.set(undefined);
  }

  public startLoading() {
    const loading: AlertItem = {
      type: 'loading',
      title: 'Cargando...',
      message: ''
    };
    this._stack.set(loading);
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
    message: string
  ) {
    const error: AlertItem = {
      type: 'error',
      title: 'Error',
      message: message
    };
    this._stack.set(error);
  }
}
