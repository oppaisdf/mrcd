import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreateUserRequest } from '../requests/create-user.requests';
import { UserResponse } from '../responses/User.response';

@Injectable()
export class UserService {
  private readonly _api = inject(ApiService);

  public toListAsync() {
    return this._api.getAsync<Array<UserResponse>>('/user');
  }

  public createAsync(
    request: CreateUserRequest
  ) {
    return this._api.postAsync<CreateUserRequest, string>('/user', request);
  }
}
