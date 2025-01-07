import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';
import { ChargeResponse } from '../models/responses/charge';
import { ChargeRequest } from '../models/requests/charge';

@Injectable()
export class ChargeService {
  constructor(
    private _api: ApiService
  ) { }

  public async GetAsync() {
    return await this._api.Get<ChargeResponse[]>('Charge');
  }

  public async GetByIdAsync(
    id: number
  ) {
    return await this._api.Get<ChargeResponse>(`Charge/${id}`);
  }

  public async CreateAsync(
    request: ChargeRequest
  ) {
    return await this._api.Post('Charge', request);
  }

  public async UpdateAsync(
    id: number,
    request: ChargeRequest
  ) {
    return await this._api.Patch(`Charge/${id}`, request);
  }

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
