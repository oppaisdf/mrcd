import { Component, OnInit } from '@angular/core';
import { RoleService } from '../../services/role.service';
import { RoleResponse } from '../../models/responses/role';

@Component({
  selector: 'role-all',
  standalone: false,

  templateUrl: './all.component.html',
  styleUrl: './all.component.sass'
})
export class AllComponent implements OnInit {
  constructor(
    private _service: RoleService
  ) { }

  roles: RoleResponse[] = [];

  async ngOnInit() {
    const response = await this._service.GetAsync();
    if (response.success) this.roles = response.data!;
  }
}
