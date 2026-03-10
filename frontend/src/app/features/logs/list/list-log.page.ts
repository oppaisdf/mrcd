import { Component, inject, OnInit, signal } from '@angular/core';
import { LogService } from '../services/log.service';
import { PagedResult } from '../../../core/api/api.types';
import { LogResponse } from '../responses/log.response';
import { AlertService } from '../../../shared/alerts/services/alert.service';

@Component({
  selector: 'app-list-log.page',
  imports: [],
  templateUrl: './list-log.page.html',
  styleUrl: './list-log.page.scss',
})
export class ListLogPage implements OnInit {
  private readonly _service = inject(LogService);
  private readonly _alert = inject(AlertService);
  readonly page = signal<PagedResult<LogResponse>>({
    items: [],
    totalCount: 0,
    page: 0,
    size: 0,
    totalPages: 0,
    hasPrevious: false,
    hasNext: false
  });

  async ngOnInit() {
    await this.loadAsync(1);
  }

  private async loadAsync(
    page: number
  ) {
    if (this._alert.loading()) return;
    this._alert.startLoading();

    const response = await this._service.toListAsync(page);
    this._alert.clear();
    if (!response.isSuccess)
      this._alert.error(response.message);
    else this.page.set(response.data!);
  }

  async changePageAsync(
    isNext: boolean
  ) {
    const page = this.page().page + (isNext ? 1 : -1);
    await this.loadAsync(page);
  }
}
