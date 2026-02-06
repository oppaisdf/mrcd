import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../../core/api/api.service';
import { CreateUserRequest } from '../requests/create-user.request';
import { UserResponse } from '../responses/User.response';
import { UpdateUserRequest } from '../requests/update-user.request';

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

  public updateAsync(
    userId: string,
    request: UpdateUserRequest
  ) {
    return this._api.patchAsync(`/user/${userId}`, request);
  }
}
