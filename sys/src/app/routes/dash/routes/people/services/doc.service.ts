import { Injectable } from '@angular/core';
import { ApiService } from '../../../../../services/api.service';

@Injectable()
export class DocService {
  constructor(
    private _service: ApiService
  ) { }

  public async AssignAsync(
    documentId: number,
    personId: number
  ) {
    return await this._service.Post(`Document/${documentId}?personId=${personId}`, {});
  }

  public async UnassignAsync(
    documentId: number,
    personId: number
  ) {
    return await this._service.Delete(`Document/${documentId}?personId=${personId}`);
  }
}
