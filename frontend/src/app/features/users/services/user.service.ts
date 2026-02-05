import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { UserDTO } from '../dtos/UserDTO';

@Injectable()
export class UserService {
  private readonly _api = inject(ApiService);

  public toListAsync() {
    return this._api.getAsync<Array<UserDTO>>('/user');
  }
}
