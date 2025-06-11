import { Injectable } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { DefaultResponse } from '../../../core/models/responses/Response';
import { DefaultRequest } from '../../../core/models/requests/Request';
import { DocumentResponse } from '../responses/document';

@Injectable()
export class DocService {
  constructor(
    private _service: ApiService
  ) { }

  public async GetAsync() {
    return await this._service.Get<DefaultResponse[]>('Document');
  }

  public async AddAsync(
    request: DefaultRequest
  ) {
    return await this._service.Post('Document', request);
  }

  public async UpdateAsync(
    id: number,
    request: DefaultRequest
  ) {
    return await this._service.Put(`Document/${id}`, request);
  }

  public async GetByIdAsync(
    id: number
  ) {
    return this._service.Get<DocumentResponse>(`Document/${id}`);
  }

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
