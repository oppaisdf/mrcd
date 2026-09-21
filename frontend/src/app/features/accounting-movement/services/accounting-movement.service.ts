import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreateAccountingMovementRequest } from '../requests/create-accounting-movement.request';

@Injectable()
export class AccountingMovementService {
  private readonly _api = inject(ApiService);

  addAsync(
    request: CreateAccountingMovementRequest
  ){
    return this._api.postAsync('/accountingmovement', request);
  }
}
