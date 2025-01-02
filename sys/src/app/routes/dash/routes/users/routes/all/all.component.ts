import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { UserResponse } from '../../models/responses/user';

@Component({
  selector: 'user-all',
  standalone: false,
  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: UserService
  ) { }

  users: UserResponse[] = [];

  async ngOnInit() {
    const response = await this._service.GetAsync();
    if (response.success) this.users = response.data!;
  }
}
