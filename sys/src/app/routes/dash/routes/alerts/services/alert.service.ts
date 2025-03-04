import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';

@Injectable()
export class AlertService {
  constructor(
    private _service: ApiService
  ) { }

  public async AlertCountAsync(
    alert: number
  ) {
    return await this._service.Get(`Alert/${alert}`);
  }
}
