import { Component, computed, input } from '@angular/core';
import { RoleDTO } from '../dtos/RoleDTO';

@Component({
  selector: 'role-permissions',
  imports: [],
  templateUrl: './role-permissions.component.html',
  styleUrl: './role-permissions.component.scss',
})
export class RolePermissionsComponent {
  role = input.required<RoleDTO>();
  permissions = computed(() => this.role().permissions);
}
