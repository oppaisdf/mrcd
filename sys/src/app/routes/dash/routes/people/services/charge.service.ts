import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';

@Injectable()
export class ChargeService {
  constructor(
    private _api: ApiService
  ) { }

  public async AssignAsync(
    chargeId: number,
    personId: number
  ) {
    return await this._api.Post(`Charge/${chargeId}/${personId}`, {});
  }

  public async UnassingAsync(
    chargeId: number,
    personId: number
  ) {
    return await this._api.Delete(`Charge/${chargeId}/${personId}`);
  }
}
