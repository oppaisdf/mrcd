import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreateAccountingMovementRequest } from '../requests/create-accounting-movement.request';
import { AccountingMovementResponse } from '../responses/accounting-movement.response';

@Injectable()
export class AccountingMovementService {
  private readonly _api = inject(ApiService);

  addAsync(
    request: CreateAccountingMovementRequest
  ){
    return this._api.postAsync('/accountingmovement', request);
  }

  toListAsync(
    filterOnlyByYear: boolean,
    date: Date
  ){
    const params: Record<string, any> = {
      filterOnlyByYear: filterOnlyByYear,
      date: new Date(date).toISOString().split('T')[0]
    }
    return this._api.getAsync<Array<AccountingMovementResponse>>(`/accountingmovement`, params);
  }
}
