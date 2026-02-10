import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { ChargeResponse } from '../responses/charge.response';
import { CreateChargeRequest } from '../requests/create-charge.request';

@Injectable()
export class ChargesService {
  private readonly _api = inject(ApiService);

  public toListAsync() {
    return this._api.getAsync<Array<ChargeResponse>>('/charge');
  }

  public deleteAsync(
    chargeId: string
  ) {
    return this._api.delAsync(`/charge/${chargeId}`);
  }

  public createAsync(
    request: CreateChargeRequest
  ) {
    return this._api.postAsync<CreateChargeRequest, string>('/charge', request);
  }
}
