import { Component, computed, HostListener, inject } from '@angular/core';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-alert',
  imports: [],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.scss',
})
export class AlertComponent {
  private readonly _service = inject(AlertService);
  readonly alert = this._service.alert;
  readonly isBlocking = computed(() => this.alert()?.type === 'loading');

  close() {
    if (this.isBlocking()) return;
    this._service.clear();
  }

  @HostListener('document:keydown.escape')
  onEscape() {
    if (this.isBlocking()) return;
    if (!this._service.isOpen()) return;
    this.close();
  }
}
